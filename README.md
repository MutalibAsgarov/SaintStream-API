\# SaintStream API



A RESTful Web API developed with ASP.NET Core for the SaintStream movie streaming platform.



\## Technologies

\- C#

\- ASP.NET Core Web API

\- Entity Framework Core

\- SQL Server



\## Features

\- Retrieve hero content

\- Retrieve active streaming platforms

\- Retrieve featured content

\- Retrieve content sections by category

\- Filter and sort content

\- Consistent API response structure



\## API Endpoints



| Method | Endpoint | Description |

|---|---|---|

| GET | `/api/home/hero` | Get hero content |

| GET | `/api/home/platforms` | Get active platforms |

| GET | `/api/home/featured` | Get featured content |

| GET | `/api/home/sections?section=movies` | Get content by section |



\### Available Sections

\- `just-release`

\- `popular-week`

\- `movies`

\- `series`

\- `korean-series`

\- `awards`

\- `fast`

\- `live`



\### Query Parameters



For the sections endpoint:



\- `section` – Required. Specifies the content section.

\- `limit` – Optional. Number of items to retrieve. Default: 10, Maximum: 20.



\## Database



The project uses SQL Server.



The database creation script is available in:



`Saint Stream API/script.sql`



\## Running the Project



1\. Clone the repository.

2\. Open the solution file.

3\. Configure the SQL Server connection string in `appsettings.json`.

4\. Run the project.





