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
```
nslookup google.com 127.0.0.1
Server:         127.0.0.1
Address:        127.0.0.1#53

Non-authoritative answer:
Name:   google.com
Address: 1.2.4.5

nslookup yahoo.co.jp 127.0.0.1
Server:         127.0.0.1
Address:        127.0.0.1#53

Non-authoritative answer:
Name:   yahoo.co.jp
Address: 9.8.7.6

nslookup o8o.jp 127.0.0.1
Server:         127.0.0.1
Address:        127.0.0.1#53

Non-authoritative answer:
Name:   o8o.jp
Address: 133.130.52.191
```
