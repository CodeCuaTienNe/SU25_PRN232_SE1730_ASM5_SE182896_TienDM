# Hướng dẫn Setup Microservice với Ocelot Gateway và RabbitMQ

## Tổng quan dự án

Dự án bao gồm 3 thành phần chính:
- **Ocelot Gateway**: API Gateway để định tuyến các request
- **Microservice 1**: Dịch vụ chính (bảng chính)
- **Microservice 2**: Dịch vụ phụ (bảng phụ)
- **RabbitMQ**: Message broker để giao tiếp giữa các microservice

## 1. Cấu hình Ocelot Gateway

### 1.1 Cài đặt package

```bash
dotnet add package Ocelot --version 16.0.1
```

### 1.2 Tạo file cấu hình ocelot.json

```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/tên api trong swagger microservice 1",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": "cổng swagger của api trên microservice 1"
        }
      ],
      "UpstreamPathTemplate": "/gateway/tên api trong swagger microservice 1",
      "UpstreamHttpMethod": [ "POST", "PUT", "GET" ]
    },
    {
      "DownstreamPathTemplate": "/api/tên api trong swagger microservice 2",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": "cổng swagger của api trên microservice 2"
        }
      ],
      "UpstreamPathTemplate": "/gateway/tên api trong swagger microservice 2",
      "UpstreamHttpMethod": [ "POST", "PUT", "GET" ]
    }        
  ]
}
```

### 1.3 Cấu hình Program.cs

```csharp
// Thêm cấu hình Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

// Middleware configuration
app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
await app.UseOcelot();
```

## 2. Cấu hình các Microservice

### 2.1 Cài đặt packages chung cho cả 2 microservice

```bash
dotnet add package MassTransit --version 8.3.0
dotnet add package MassTransit.RabbitMQ --version 8.3.0
dotnet add package MassTransit.AspNetCore --version 7.3.1
```

## 3. Microservice 1 (Bảng chính)

### 3.1 Cấu hình Program.cs

```csharp
// Logging configuration
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

// MassTransit configuration
builder.Services.AddMassTransit(x =>
{
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    }));
});
```

### 3.2 Tạo Controller

- Tạo Controller với template **Controllers (Read/Write Action)**
- Tham khảo code mẫu trong repository

## 4. Microservice 2 (Bảng phụ)

### 4.1 Tạo Consumer class

```csharp
public class HealthRecordsAnhNTVConsumer : IConsumer<HealthRecordsAnhNtv>
{
    private readonly ILogger<HealthRecordsAnhNTVConsumer> _logger;
    
    public HealthRecordsAnhNTVConsumer(ILogger<HealthRecordsAnhNTVConsumer> logger)
    {
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<HealthRecordsAnhNtv> context)
    {
        var healthRecord = context.Message;
        if(healthRecord != null)
        {
            string messageLog = string.Format("[{0}] CONSUME data from RabbitMQ.healthRecordsAnhNtvQueue: {1}", 
                DateTime.Now.ToString(), 
                Utilities.ConvertObjectToJsonString(healthRecord));
            Utilities.WriteLoggerFile(messageLog);
            _logger.LogInformation(messageLog);
        }
    }
}
```

### 4.2 Tạo Controller

Tạo Controller tương tự như Microservice 1

### 4.3 Cấu hình Program.cs

```csharp
// Logging configuration
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

// MassTransit configuration
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ServiceNhanVTsConsumer>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("serviceNhanVTQueue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<ServiceNhanVTsConsumer>(provider);
        });
    }));
});
```

## 5. Cấu hình RabbitMQ

### 5.1 Khởi động RabbitMQ Service

1. Khởi động RabbitMQ server trên local
2. Truy cập Management UI: `http://localhost:15672/#/`

### 5.2 Thông tin đăng nhập

- **Username**: `guest`
- **Password**: `guest`

> **Lưu ý**: Giữ RabbitMQ Management UI mở trong suốt quá trình test để theo dõi message flow

## 6. Chạy và Test hệ thống

### 6.1 Cấu hình Multiple Startup Projects

1. Set up multiple startup projects trong Visual Studio
2. Chạy đồng thời cả 3 projects: Ocelot Gateway, Microservice 1, Microservice 2

### 6.2 Test API endpoints

1. **Ocelot Gateway** sẽ có các API configuration bao gồm:
   - 2 API POST/GET
   - 1 API DELETE
   - 1 API mặc định WeatherForecast

### 6.3 Kiểm tra kết quả

1. Test bất kỳ endpoint nào trên Microservice 1 (khuyến nghị thử với Create endpoint)
2. Quan sát RabbitMQ Management UI:
   - Biểu đồ message queue sẽ nhảy lên khi có message
   - Response trả về OK
3. Nếu cả hai điều kiện trên đều thỏa mãn → Setup thành công

## 7. Lưu ý quan trọng

- Đặt tên các file microservices theo tên trong repository
- Reference đến lớp model tương ứng
- Sử dụng code tham khảo có sẵn trong repository
- Không cần thay đổi username/password mặc định của RabbitMQ

## Troubleshooting

- Kiểm tra RabbitMQ service đã được khởi động
- Đảm bảo các port không bị conflict
- Kiểm tra logging để debug các vấn đề
- Verify message flow trong RabbitMQ Management UI
