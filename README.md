# dnsd
127.0.0.1に送られてくる正引きの問い合わせに対して, setting.csvに書かれたドメインの場合は対応する任意のIPを返答します  
それ以外の場合は8.8.8.8に問い合わせて, その返答を返します  
あくまで実験用なので, Browsingなどの実用には耐えられません, 気を付けて下さい  

## Usage
- setting.csvにドメイン名とIPアドレスをカンマ区切りで記入して下さい
```
google.com, 1.2.3.4
yahoo.co.jp, 9.8.7.6
```
- Program.csをcsc等でビルドして下さい
```
csc Program.cs
```
- ビルドしたプログラムをsetting.csvのあるディレクトリで起動して下さい
- 任意のドメインに関する正引きの返答が任意の値になります  
それ以外の問い合わせには8.8.8.8の返答を返します  
<img src="http://c.arckty.org/dnsd.png" width="40%">  
```
Address:127.0.0.1
Port:56501
Received.Data:
E0 40 01 00 00 01 00 00 00 00 00 00 03 6F 38 6F
02 6A 70 00 00 01 00 01
Domain:o8o.jp
Forward Response.Data:
E0 40 81 80 00 01 00 01 00 00 00 00 03 6F 38 6F
02 6A 70 00 00 01 00 01 C0 0C 00 01 00 01 00 00
08 CD 00 04 85 82 34 BF


Address:127.0.0.1
Port:56504
Received.Data:
1C 49 01 00 00 01 00 00 00 00 00 00 06 67 6F 6F
67 6C 65 03 63 6F 6D 00 00 01 00 01
Domain:google.com
My Response.Data:
1C 49 81 80 00 01 00 01 00 00 00 00 06 67 6F 6F
67 6C 65 03 63 6F 6D 00 00 01 00 01 C0 0C 00 01
00 01 00 01 51 80 00 04 01 02 04 05


Address:127.0.0.1
Port:56507
Received.Data:
AB 4F 01 00 00 01 00 00 00 00 00 00 05 79 61 68
6F 6F 02 63 6F 02 6A 70 00 00 01 00 01
Domain:yahoo.co.jp
My Response.Data:
AB 4F 81 80 00 01 00 01 00 00 00 00 05 79 61 68
6F 6F 02 63 6F 02 6A 70 00 00 01 00 01 C0 0C 00
01 00 01 00 01 51 80 00 04 09 08 07 06
```
