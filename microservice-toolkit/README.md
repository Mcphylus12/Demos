# Microservice Toolkit
A proof of concept for nuget packages to facilitate a c sharp application written in microservices.
The aim is to trivialise common tasks like loading config from a central source and 
sync/async communication between services.

Some features are covered by common infrstructure level tools/feature but this repo aims to achieve everything
agnostic of the deployment environment.

## Features/Ideas
Support will be primarily built around the Microsoft Hoting Model but the idea is to be generic as possible to
support as many scenarios as possible

### Service Communication
- Provide support for fire and forget messaging as well as request response
- Servers make 1 call on startup
- Clients inject 1 service with `Send` overloads for request/response and Fire and Forget
- Interfaces for wide support of both comm types
- Type and string based sending and receiving
- Listener registration via reflection, code gen or manual

### Monitoring
- Support for tracing, logging and metrics
- Single unified service for sending any monitoring information
- built on ILogger for library support
- 3 Interfaces for wide support `ILoggingProvider, IMetricsProvider, ITraceProvider`
- support Count (Track Occurence), Gauge (Track Delta), and Histogram (Track Value)
- support unified tagging/scoping on all information

### Caching
- Type and string based key value caching
- Support `ICache` or `ICache<TKey, TValue>`
- Work with `ICacheProvider` Interface to allow local and distributed caching
- Avoid objects in the cache needing a particular contract

### Configuration and Service Discovery
- Service with API for GetConfig(ServiceKey)
- Api supports RegisterService(ServiceKey, Location?) that defaults to the callers IP
- Support "not ready" is service config involves key from unregistered service
- Support ServiceKey being namespaced(Global.MessagingService.DemoService1) and config being inherited like logging config
- Custom ConfigurationSource for integrating with IConfiguration with support for IOptionsMonitor and IOptionsSnapshot
- Api that would allow a frontend/api client to modify config

### Authorisation/Authentication/Identity
- Custom Job like OIDC
- Support Claims and Way to validate tokens properly

### Service Health
- Service that pings other services for health
- StartupHelpers to configure healthchecks

### Documentation
- Tool/Nuget to generate docfx

## Demo
Create real world project to demo all features using docker compose

## Delivery
- Nuget packages and metapackage to add all features


