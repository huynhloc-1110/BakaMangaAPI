# BakaMangaAPI

## Project Description

3kManga is the final project for our Bachelor Program in University of Greenwich
in Ho Chi Minh City Campus. It is a social media website for sharing and reading
comic. The website's goal is to eradicate a common issue with comic websites,
which is relying too much on a separate forum or third-party services to provide
discussion space for readers.

Our website solve the problem by having its own community section where users
can share their opinion on the comics. The core features of the website include:

- Browsing and reading comic
- Uploading comic
- Community features
- Administering data

## Repository Description

This repository is the back-end ASP.NET Web API for the 3kManga website. To view
the front-end ReactJS, please visit
[here](https://github.com/huynhloc-1110/bakamanga-react).

This Web API use the following technologies for development:

- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Cloudinary

## Deployment link

We have deployed the API with Render [here](https://bakamanga-api.onrender.com).
You can also visit the website on Vercel [here](https://bakamanga.vercel.app).

## Run locally

To run the API locally, follow the steps below:

1. Clone the project.

```shell
git clone git@github.com:huynhloc-1110/BakaMangaAPI.git
```

2. Change the shell location to the inner BakaMangaAPI directory. Then, set the
   default connection string in your user secret to your PostgreSQL local
   instance.

```shell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=5432;Database=<database_name>;User Id=<user_id>;Password=<password>"
```

3. Init the database.

```shell
dotnet ef database update
```

4. Run the project.

```shell
dotnet run
```

When run locally, data is stored in the PostgreSQL local instance and images are
stored in the `wwwroot` folder.
