---
title: About
---

<script setup>
import { data } from '../dependencies.data.ts'
import Licenses from '../components/Licenses.vue'
import { VPTeamMembers } from 'vitepress/theme'

const members = [
  {
    avatar: 'https://avatars.githubusercontent.com/u/19796163',
    name: 'Ismail Bennani',
    title: 'Creator',
    links: [
      { icon: 'github', link: 'https://github.com/ismailbennani' },
      { icon: 'linkedin', link: 'https://www.linkedin.com/in/lbismail/' }
    ]
  },
];
</script>

# About

## About me

<div class="text-center">
    <VPTeamMembers :members />
</div>

Hi! I’m Ismail Bennani, the creator and maintainer of FacturX.NET.
This project began as a personal challenge to build a fast and resource-efficient library for handling Factur-X invoices. Over time, it evolved into a clean, modern, and developer-friendly toolkit designed to make working with Factur-X as smooth as possible.

I’ve poured a lot of time and care into building this library, and I’m excited to keep improving it with your feedback and ideas.

If you’d like to get in touch, contribute, or support the project:

- <i class="bi bi-envelope-at"></i> [Contact me](mailto:contact@facturxdotnet.org) – Feel free to reach out if you have questions or suggestions
- <i class="bi bi-chat-dots"></i> [Open an issue](https://github.com/FacturX-NET/FacturXDotNet) – Found a bug or have a feature request? Let me know on GitHub
- <i class="bi bi-star"></i> [Star the project](https://github.com/FacturX-NET/FacturXDotNet) – Show your support by starring FacturX.NET on GitHub

Thanks for checking out the project – and happy invoicing! 🚀

## Dependencies

FacturX.NET is built on top of reliable open-source technologies.
To ensure transparency and maintainability, the dependencies used across the project are grouped by component and sorted by license type, making it easier to review or audit them if needed.

Here’s a breakdown of the main dependencies for each part of the project:


### Documentation website (this website)

<Licenses :dependencyGroups="data.docs" />

### Web editor app

<Licenses :dependencyGroups="data.editor" />

### API

<Licenses :dependencyGroups="data.api" />

### CLI

<Licenses :dependencyGroups="data.api" />

### Library

<Licenses :dependencyGroups="data.library" />