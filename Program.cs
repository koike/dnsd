using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Threading;

namespace dnsd
{
    class Program
    {
        static void Main(string[] args)
        {
            var setting =
                new StreamReader("setting.ini", Encoding.UTF8).ReadToEnd().Trim()
                    .Split('\n')
                    .Select(i => i.Trim().Split(',').ToList())
                    .ToList();

            var ip = "127.0.0.1";
            var port = 53;
            while (true)
            {
                var local = IPAddress.Parse(ip);

                var ep = new IPEndPoint(local, port);
                var udp = new UdpClient(ep);

                try
                {
                    IPEndPoint remote = null;
                    var rcv = udp.Receive(ref remote);

                    Console.WriteLine($"Address:{remote.Address}, Port:{remote.Port}");
                    Console.WriteLine($"Received.Data => {BitConverter.ToString(rcv).Replace("-", " ")}");

                    var res = new List<byte>();
                    var domainBytes = new List<byte>();

                    //Transaction ID
                    res.Add(rcv[0]);
                    res.Add(rcv[1]);

                    //Flags
                    //0x8180 is Reply Code: No error (0)
                    res.Add(0x81);
                    res.Add(0x80);

                    //Questions: 1
                    res.Add(0x00);
                    res.Add(0x01);

                    //Answer RRs: 1
                    res.Add(0x00);
                    res.Add(0x01);

                    //Authority RRs: 0
                    res.Add(0x00);
                    res.Add(0x00);

                    //Aditional RRs: 0
                    res.Add(0x00);
                    res.Add(0x00);

                    //Name
                    for (var i = 12; i < rcv.Length - 4; i++)
                    {
                        res.Add(rcv[i]);
                        if (rcv[i] <= 0x20)
                        {
                            domainBytes.Add(0x2E);
                        }
                        else
                        {
                            domainBytes.Add(rcv[i]);
                        }
                    }

                    //Type: A => 1
                    res.Add(0x00);
                    res.Add(0x01);

                    //Class: IN => 1
                    res.Add(0x00);
                    res.Add(0x01);

                    //domainを得る
                    domainBytes.RemoveAt(0);
                    domainBytes.RemoveAt(domainBytes.Count - 1);
                    var domainName = Encoding.UTF8.GetString(domainBytes.ToArray());
                    Console.WriteLine($"Domain => {domainName}");

                    //Name: 固定値？
                    res.Add(0xc0);
                    res.Add(0x0c);

                    //Type: A => 1
                    res.Add(0x00);
                    res.Add(0x01);

                    //Class: IN => 1
                    res.Add(0x00);
                    res.Add(0x01);

                    //Time to live: 86400
                    res.Add(0x00);
                    res.Add(0x01);
                    res.Add(0x51);
                    res.Add(0x80);

                    //Data length: 4
                    res.Add(0x00);
                    res.Add(0x04);

                    var count = setting.Count(s => s[0].Trim().Equals(domainName));

                    //domainが特定のものだった場合
                    if (count > 0)
                    {
                        foreach (var _ in from list in setting
                                          where list[0].Trim().Equals(domainName)
                                          select list[1].Trim().Split('.').Select(int.Parse).Select(BitConverter.GetBytes).ToArray())
                        {
                            res.AddRange(_.Select(b => b[0]));
                        }
                        udp.Send(res.ToArray(), res.Count, remote.Address.ToString(), remote.Port);
                        Console.WriteLine($"Response.Data => {BitConverter.ToString(res.ToArray()).Replace("-", " ")}");
                        udp.Close();
                    }
                    else
                    {
                        var google = new UDP("8.8.8.8", 44444);
                        google.Send(rcv);
                        var googleRes = google.Receive();
                        udp.Send(googleRes, googleRes.Length, remote.Address.ToString(), remote.Port);
                        Console.WriteLine($"Response.Data => {BitConverter.ToString(googleRes.ToArray()).Replace("-", " ")}");
                        google.Close();
                        udp.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    udp.Close();
                }
            }
        }
    }

    class UDP
    {
        private UdpClient udp;
        public UDP(string hostname, int port)
        {
            udp = new UdpClient(port);
            udp.Connect(hostname, 53);
        }

        public void Send(byte[] message)
        {
            udp.Send(message, message.Length);
        }

        public byte[] Receive()
        {
            var remote = new IPEndPoint(IPAddress.Any, 0);
            return udp.Receive(ref remote);
        }

        public void Close()
        {
            udp.Close();
        }
    }
}
