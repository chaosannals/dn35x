# Demo

## OpenSSL

```bash
# 生成 dn35x.key dn35x cert
openssl req -newkey rsa:2048 -nodes -keyout dn35x.key -x509 -days 365 -out dn35x.cer

# 生成 dn35x.pfx
openssl pkcs12 -export -in dn35x.cer -inkey dn35x.key -out dn35x.pfx
```
