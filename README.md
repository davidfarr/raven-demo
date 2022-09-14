# Raven Demo Application

## Project Prompt
An SPA-style web application that allows users to create projects, software requirements, and test cases. Users can quickly and easily build up a list of requirements and test cases for each requirement so that they can quickly discover test coverage on requirements.

## Highlights
This demo application is not meant to be a final product, but as a first iteration explores:
- Microservice architecture
    - Uses distinct, containerized minimal API services for a web application service consumer
- Use of jQuery to simulate single page applications
    - Vue.js would be a better permanent solution! jQuery is fast for my prototyping, and helps demonstrate comfort with selectors and spans fundamental to more advanced work with the platform
- Use of Cloud Database
    - Accomplished by connecting to an Azure Microsoft MS SQL database
- Clean UX
    - Building on the Bootstrap framework, this application customizes the UI/UX to make sure that it's easy to navigate and work with controls, making sure that things make sense for a user that is used to working with very complicated tools to do these same tasks.

## Stack
- .NET 6.0 Minimal API
- .NET Core Razor Web Application
- jQuery for AJAX, etc.
- Bootstrap front-end
- Azure MS SQL Database
- EntityFramework Core 6
- Docker (Linux-based)

## Getting Started on Your Machine
1. Clone the repo locally
2. Set up your own Azure SQL DB or use a local SQL instance (more configuration needed, particularly for Docker)
3. Run any scripts in the schema folder to initialize the database properly
4. Build Docker images (3 API services + web application) (note, advise to run on localhost as there is no authentication in this demo, and CORS settings are modified for localhost use)
5. Update localhost:port hardcoded strings in the site.js file of Raven.Web to match Docker container ports for the appropriate APIs.
6. Run all 4 Docker images and navigate to web application index page. 

## Main Features
### Create Projects
[Video screengrab](/screengrabs/CreatingProject.gif)
![Video gif of a project being created in application](/screengrabs/CreatingProject.gif)

### Create Software Requirements
[Video screengrab](/screengrabs/CreatingRequirement.gif)
![Video gif of software requirements being created in application](/screengrabs/CreatingRequirement.gif)
Requirements can also be modified!

### Create Test cases
[Video screengrab](/screengrabs/CreatingTestCases.gif)
![Video gif of test cases being created in application](/screengrabs/CreatingTestCases.gif)
Test cases can also be modified!

## Next Steps
In the future I plan to implement the following in order of priority (as time permits):
1. UI polish pass on modifying requirements, test cases
2. Modify projects
3. Delete projects, requirements, test cases
4. Refactor test coverage dynamic tags
5. Unit tests (NUnit)
6. Print reports
    1. Print every requirement for a project
    2. Print every test case for a project
    3. Print every test case for a requirement
    4. Print every requirement and project that has no test coverage
7. Robot Framework or Playwright front-end testing
8. Switch from .NET Core Razor web app to Vue.js

