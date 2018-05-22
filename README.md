# dotNetCore快速开发框架

该框架为基于EF Core的 .Net Core2.0快速开发框架

**SKD**
- .Net Core 2.0

**数据库支持**
- SQL Server
- MySql
- PostgreSQL

**缓存**
- Redis
- Memory Cache

**日志**
- log4net

>**Begin Start**
>- 在Web项目中创建appsettings.json文件（基于appsettings.templete）
>- 修改Web项目中的Startup的仓储层、业务层的程序集注入名称

>**Doing**
>- 基于Identityserver4的身份认证，采用Bearer模式，采用JWT规范。单独部署[认证服务](https://gogs.danggui.fun/ferdinandhuang/AuthPlatform)

>**TODO**
>- 添加RabbitMQ消息队列
>- 添加DBFirst支持(Dapper/Chole)
>- 待定