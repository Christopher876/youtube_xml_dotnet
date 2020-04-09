# Youtube Notifier

## Restoring the videos database

Install ef core tools if not already installed
```bash
dotnet tool install --global dotnet-ef
```

```bash
dotnet ef database update
```

## If you encounter an error when sending emails from Gmail

- Firstly, I recommend creating a new account and then setting all of that account's information up in email-login.json. The reason for this is because the setting required without using the API is less secure (according to Google) and thus they block it by default.
- Head to https://myaccount.google.com/u/5/lesssecureapps?pli=1 with that new account or your existing account and turn that setting ON.