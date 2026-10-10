# Educational gRPC host

This opt-in .NET 10 host exposes actual generated protobuf `baseline.sample.items.v1.Items` `Get` and `Create` methods from `Protos/items.proto`. `Grpc.AspNetCore` supplies the server runtime, protobuf serialization and build-time code generator; the project compiles server contracts with `GrpcServices="Server"`.

The host dispatches application requests through MediatR and uses its own volatile in-memory store. It does not share state with the separate WebApi process and has no external-service dependencies. `Create` accepts only `name`; ownership comes exclusively from the authenticated server principal. `Get` hides other owners' items as `NotFound`. Invalid UUIDs or names return `InvalidArgument`.

## Safety boundary

Startup requires `Development`; all other environments fail before serving requests. The host owns its cleartext HTTP/2 listener at `127.0.0.1:5081`, rejects configured `Kestrel:Endpoints`, and overrides hosting URL settings. This is not a production TLS or identity implementation.

Authentication fails closed by default. Local educational identity simulation requires explicit `DemoAuth:Enabled=true` configuration and one `X-Demo-User` metadata value containing 1–64 ASCII letters, digits, underscores or hyphens. Metadata never supplies a protobuf owner field. Both methods require authorization, and the service independently checks the authenticated name-identifier claim. No credentials are required or provided by this module.

Production requires a separately approved TLS endpoint and real authentication configuration; do not enable this demo outside its loopback educational boundary. There is no reflection service, production listener, durable storage, health certification or external integration claim.

## Build-only validation

From the repository root, build the project with `dotnet build examples/Baseline.Sample/src/Presentation/Baseline.Sample.Grpc/Baseline.Sample.Grpc.csproj`. Package versions are owned by the sample's central package manifest. This command generates protobuf contracts and compiles the host without opening a socket. Runtime/client interoperability, authorization and HTTP/2 socket tests require separate execution and are not claimed by a successful build.
