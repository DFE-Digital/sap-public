<a name="readme-top"></a>

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]




<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/DFE-Digital/sap-public/">
    <img src="docs/_assets/logo.png" alt="Logo" width="80" height="80">
  </a>

<h3 align="center">School Improvement Programme - School Profiles (working title)</h3>

  <p align="center">
    A WIP project 
    <br />
    <br />
    <a href="#">View</a>
    ·
    <a href="https://github.com/DFE-Digital/sap-public/issues/new?labels=bug&template=bug-report---.md">Report Bug</a>
    ·
    <a href="https://github.com/DFE-Digital/sap-public/issues/new?labels=enhancement&template=feature-request---.md">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

TBC

| Environment | URL | Status | Code Coverage
| --- | --- | --- | --- |
| Production | https://sap-public-production.teacherservices.cloud/ | ![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/DFE-Digital/sap-public/dotnet-prod.yml) | TBC | TBC
| Test | https://sap-public-test.test.teacherservices.cloud/  | ![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/DFE-Digital/sap-public/build-and-deploy.yml?main) | TBC |


<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With
<a href="https://docs.microsoft.com/en-us/dotnet/csharp/"><img src="https://img.shields.io/badge/language-C%23-%23178600" title="Go To C# Documentation"></a>

<a href="https://github.com/DFE-Digital/sap-public"><img src="https://img.shields.io/badge/github-repo-%2324292e?logo=github" title="Go To Github Repo"></a>


<a href="https://dotnet.microsoft.com/download"><img src="https://img.shields.io/badge/dynamic/xml?color=%23512bd4&label=target&query=%2F%2FTargetFramework%5B1%5D&url=https://raw.githubusercontent.com/DFE-Digital/sap-public/main/SAPPub.Web/SAPPub.Web.csproj&logo=.net" title="Go To .NET Download"></a>




<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 22.x](https://nodejs.org/) (for building frontend assets)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional, for containerized development)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) (recommended)

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/DFE-Digital/sap-public.git
cd sap-public
```

### 2. Install .NET dependencies

```bash
dotnet restore
```

### 3. Install Node.js dependencies (for frontend assets)

```bash
cd SAPPub.Web
npm install
cd ..
```

The `npm install` command automatically runs a `postinstall` script that copies GOV.UK Frontend and required libraries from `node_modules` to `wwwroot/lib/`.

## Running Locally

### Option 1: Using .NET CLI

```bash
cd SAPPub.Web
dotnet run
```

The application will be available at `http://localhost:3000`

### Option 2: Using Visual Studio

1. Open `sap-public.sln` in Visual Studio
2. Press `F5` to run with debugging (or `Ctrl+F5` without debugging)
3. The application will launch in your default browser

### Option 3: Using VS Code

1. Open the project folder in VS Code
2. Press `F5` to start debugging
3. Select ".NET Core Launch (web)" configuration
4. Navigate to `http://localhost:3000`

## Running with Docker

### Build the Docker image

```bash
docker build -t sappub:latest .
```

The Docker build process:
1. **Assets stage**: Builds frontend assets using Node.js
2. **Build stage**: Compiles .NET application
3. **Final stage**: Creates minimal runtime image

### Run the container

```bash
docker run -p 3000:3000 sappub:latest
```

The application will be available at `http://localhost:3000`


<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Project Link: [https://github.com/DFE-Digital/sap-public](https://github.com/DFE-Digital/sap-public)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/DFE-Digital/sap-public.svg?style=for-the-badge
[contributors-url]: https://github.com/DFE-Digital/sap-public/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/DFE-Digital/sap-public.svg?style=for-the-badge
[forks-url]: https://github.com/DFE-Digital/sap-public/network/members
[stars-shield]: https://img.shields.io/github/stars/DFE-Digital/sap-public.svg?style=for-the-badge
[stars-url]: https://github.com/DFE-Digital/sap-public/stargazers
[issues-shield]: https://img.shields.io/github/issues/DFE-Digital/sap-public.svg?style=for-the-badge
[issues-url]: https://github.com/DFE-Digital/sap-public/issues
[license-shield]: https://img.shields.io/github/license/DFE-Digital/sap-public.svg?style=for-the-badge
[license-url]: https://github.com/DFE-Digital/sap-public/blob/master/LICENSE.txt

[language-shield]: https://img.shields.io/badge/dynamic/xml?label=target&query=%2F%2FTargetFramework%5B1%5D&url=https://raw.githubusercontent.com/DFE-Digital/sap-public/main/Charybdis.Web/Charybdis.Web.csproj&logo=.net?style=for-the-badge

[language-url]: https://learn.microsoft.com/en-us/dotnet/csharp/