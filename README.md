# AWS Secrets Manager (VB.NET 4.8)

Prints `secret1` and `secret2` using `New AmazonSecretsManagerClient()` (same credentials/region as the AWS CLI, e.g. `~/.aws/credentials`).

```bash
export AWS_REGION=us-east-2   # if needed
dotnet run
```

Needs .NET SDK or VS with .NET 4.8 targeting pack.
