# AWS Secrets Manager — VB.NET example

Tiny **.NET Framework 4.8** console app: reads two secrets by name using the **AWS SDK default client** (same credentials and region behavior you get with the AWS CLI, e.g. `~/.aws/credentials` on your PC).

## What you need

1. **.NET SDK** (to build/run with `dotnet`) *or* Visual Studio with **.NET Framework 4.8 targeting pack**.
2. **AWS access** to Secrets Manager in some account/region.
3. **Secrets** named `secret1` and `secret2` in that account.

## Configure AWS on your machine

Use either:

- **`aws configure`** (install [AWS CLI](https://aws.amazon.com/cli/)), which writes `~/.aws/credentials` and `~/.aws/config`, **or**
- Environment variables such as `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`, and **`AWS_REGION`** (or `AWS_DEFAULT_REGION`).

The line `New AmazonSecretsManagerClient()` does **not** pass keys in code; the SDK loads them the usual way.

## Region

If you get errors about a missing region, set it explicitly, for example:

```bash
export AWS_REGION=us-east-2
```

On Windows PowerShell:

```powershell
$env:AWS_REGION = "us-east-2"
```

You can also set `region = ...` in `~/.aws/config` for your profile.

## Run

From this folder:

```bash
dotnet run
```

Or build then run the `.exe` under `bin/Debug/net48/` (or `Release` after `dotnet build -c Release`).
