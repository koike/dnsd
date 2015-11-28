# dnsd
127.0.0.1に送られてくる任意のドメインに関する正引きの問い合わせに対して, 任意の値を返答します  
Browsingなどの実用には耐えられないと思います  

## Usage
- setting.iniにドメイン名とIPアドレスをカンマ区切りで記入して下さい
```
google.com, 127.0.0.1
yahoo.co.jp, 8.8.8.8
```
- Program.csをcsc等でビルドして下さい
```
csc Program.cs
```
- ビルドしたプログラムをsetting.iniのあるディレクトリで起動して下さい
- 任意のドメインに関する正引きの返答が任意の値になります  
それ以外の問い合わせには8.8.8.8の返答を返します
```
nslookup google.com 127.0.0.1
Server:         127.0.0.1
Address:        127.0.0.1#53

Non-authoritative answer:
Name:   google.com
Address: 127.0.0.1

nslookup yahoo.co.jp 127.0.0.1
Server:         127.0.0.1
Address:        127.0.0.1#53

Non-authoritative answer:
Name:   yahoo.co.jp
Address: 8.8.8.8

nslookup o8o.jp 127.0.0.1
Server:         127.0.0.1
Address:        127.0.0.1#53

Non-authoritative answer:
Name:   o8o.jp
Address: 133.130.52.191
```
