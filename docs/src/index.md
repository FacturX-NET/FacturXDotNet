---
# https://vitepress.dev/reference/default-theme-home-page
layout: home

title: FacturX.NET Documentation

hero:
  name: FacturX.NET
  text: Web, API, CLI, .NET — your workflow, your way
  tagline: FacturX.NET gives you a unified platform to manage Factur-X documents the way you prefer.
  image: { src: '/logo.png', alt: 'FacturX.NET Logo' }
  actions:
    - theme: brand
      text: What is FacturX.NET?
      link: /guides/what-is-facturxdotnet
    - theme: alt
      text: Self-hosting
      link: /guides/self-hosting
    - theme: alt
      text: GitHub
      link: https://github.com/FacturX-NET/FacturXDotNet

features:
  - title: Web Editor
    icon: { 
      light: 'https://api.iconify.design/bi/globe2.svg?color=%23512bd4', 
      dark: 'https://api.iconify.design/bi/globe2.svg?color=%23CAC0F2', 
      alt: 'WebSite' 
    }
    details: Create, view, and edit Factur-X documents directly in your browser with our user-friendly editor.
    link: $env.editor.url
    linkText: Try it live
  - title: API
    icon: { 
      light: 'https://api.iconify.design/simple-icons/amazonapigateway.svg?color=%23512bd4', 
      dark: 'https://api.iconify.design/simple-icons/amazonapigateway.svg?color=%23CAC0F2', 
      alt: 'API' 
    }
    details: Programmatically generate, read, and validate Factur-X documents through our powerful API.
    link: $env.api.url
    linkText: Try it live
  - title: .NET library and tool
    icon: { 
      light: 'https://api.iconify.design/simple-icons/nuget.svg?color=%23512bd4', 
      dark: 'https://api.iconify.design/simple-icons/nuget.svg?color=%23CAC0F2', 
      alt: 'NuGet' 
    }
    details: Integrate FacturX.NET into your .NET projects or use the CLI to automate workflows directly from the command line.
    link: https://www.nuget.org/profiles/FacturX.NET
    linkText: Download
---