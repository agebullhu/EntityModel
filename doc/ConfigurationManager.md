# ConfigurationManager
> 逐步放弃NetFramework故剽窃了原ConfigurationManager名称,使用相似的接口访问.

## 使用哪个配置文件

1. 默认使用appsettings.json
> Builder属性自动构造时,会查找当前目录下是否存在appsettings.json,如果存在则默认载入.

2. 指定文件
> 调用Load文件,传入Json文件名称(注意,不会校验是否存在),并且会自动和之前载入的文件合并,
如果在调用Load之前调用过任何属性或方法,都会使用默认(1)的appsettings.json

## 使用根配置
> Core一般把全局变量方在根配置,为防止配置污染,请尽量不要使用.

通过调用 Root 下标方式访问

## 使用AppSettings节

实例对象:AppSettings
1. this[]
2. GetAppSetting
3. GetAppSettingInt
4. GetAppSettingLong
5. GetAppSettingDouble
6. GetAppSettingDecimal
7. SetAppSetting

## 使用ConnectionStrings节
> 配置不同于之前的方式,与AppSettings不同仅所在在的配置节不同

实例对象:ConnectionStrings
1. this[]
2. GetConnectionString
3. SetConnectionString