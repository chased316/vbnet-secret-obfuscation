# AWS Secrets Manager (VB.NET 4.8)

Demo reads **AWS access key**, **secret key**, and **region** from **`real.config`** (same shape as `App.config.example`). Secret names **`secret1`** and **`secret2`** stay hardcoded in `Program.vb`.

## Setup

1. Copy **`App.config.example`** to **`real.config`** in this folder (same folder as the `.vbproj`).
2. Replace the placeholder values with your keys and region.
3. **`real.config` is gitignored**—do not commit it.

```bash
dotnet run
```

`real.config` is copied to the build output when it exists so the app can load it next to the `.exe`.

Needs .NET SDK or VS with .NET 4.8 targeting pack.

**Security:** never commit access keys. If keys were ever shared or committed by mistake, **rotate them in IAM** and update `real.config` locally.
