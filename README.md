# Phone Directory Application

This project involves designing a structure with a minimum of two microservices communicating with each other to create a simple phone directory application.

## Table of Contents
1. [Purpose](#purpose)
2. [Technical Details](#technical-details)
3. [Technologies Used](#technologies-used)
4. [Installation](#installation)
5. [Services and APIs](#services-and-apis)
6. [Report Request](#report-request)

## Purpose

This project creates a phone directory application providing basic functionality such as contact management, adding/removing communication information, statistical report requests based on the location of contacts, and more.

## Technical Details

- Person data structure: UUID, First Name, Last Name, Company, Contact Information (Info Type, Info Content)
- Report data structure: UUID, Requested Date, Report Status (In Progress, Completed)

## Technologies Used

- .NET Core
- Git
- MongoDB
- RabbitMQ (Message Queue)


## Services and APIs

- **Contact API:**
  - Provides basic functionality such as creating and removing contacts, adding/removing communication information, and listing contacts.

- **Report API:**
  - Provides functionality for report requests, listing system-generated reports, and accessing their details.

## Report Request

- Report requests work asynchronously.
- When a user requests a report, the system queues the request. Once completed, the user can observe the report through the "list of reports" endpoint.

## Testing

To run the tests, use the following command:

```bash
dotnet test
