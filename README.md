# AWS Secrets Manager (VB.NET 4.8)

`Program.vb` hardcodes **region** `us-east-2` and secret names **`secret1`** / **`secret2`**. Only **AWS credentials** come from your machine (same chain as the CLI, e.g. `~/.aws/credentials`).

```bash
dotnet run
```

Needs .NET SDK or VS with .NET 4.8 targeting pack.
