# StackExchangeRedis
> IRedis的StackExchange实现,此烂货有个问题,就是连接频繁开关会导致连接数不足,所以建议使用单例模式注册

## 配置

配置节:ConnectionStrings

 Redis : 连接字符串,默认为127.0.0.1:6379
 
- 如果有密码,使用 password=pwd
> 如:127.0.0.1:6379,password=123456
- syncTimeout:连接超时
- abortConnect:中止连接

```json
{
	"ConnectionStrings":{
		"Redis":"127.0.0.1:6379,password=123456,syncTimeout=3000,abortConnect=true"
	}
}