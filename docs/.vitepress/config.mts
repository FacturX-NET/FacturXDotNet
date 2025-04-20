import { defineConfig } from "vitepress"; // https://vitepress.dev/reference/site-config

// https://vitepress.dev/reference/site-config
export default defineConfig({
  srcDir: "./src",
  head: [["link", { rel: "icon", href: "/favicon.ico" }]],

  title: "FacturX.NET Documentation",
  description: "Work with Factur-X documents in .NET",

  themeConfig: {
    // https://vitepress.dev/reference/default-theme-config
    nav: [
      { text: "Home", link: "/" },
      { text: "Getting started", link: "/getting-started" },
      { text: "OpenAPI specification", link: "/api/openapi-specification" },
      { text: ".NET API reference", link: "/library/api-reference" },
    ],

    sidebar: [
      {
        text: "Getting started",
        link: "/getting-started",
      },
      {
        text: "Use cases",
        items: [
          {
            text: "Generation",
            items: [
              {
                text: "Generate a Factur-X document",
                link: "/generation/facturx",
              },
              {
                text: "Generate a standard PDF",
                link: "/generation/standard-pdf",
              },
            ],
          },
          {
            text: "Validation",
            items: [
              {
                text: "Validate a Factur-X document",
                link: "/validation/facturx",
              },
              {
                text: "Validate Cross-Industry Invoice data",
                link: "/validation/cii",
              },
            ],
          },
          {
            text: "Extraction",
            items: [
              {
                text: "Extract Cross-Industry Invoice data",
                link: "/extraction/cii",
              },
              { text: "Extract XMP metadata", link: "/extraction/cii" },
            ],
          },
        ],
      },
      {
        text: "API",
        items: [
          { text: "OpenAPI specification", link: "/api/openapi-specification" },
        ],
      },
      {
        text: "CLI",
        items: [{ text: "CLI usage", link: "/cli/cli-usage" }],
      },
      {
        text: ".NET library",
        items: [{ text: "API reference", link: "/library/api-reference" }],
      },
    ],

    socialLinks: [
      { icon: "github", link: "https://github.com/FacturX-NET/FacturXDotNet" },
      { icon: "nuget", link: "https://www.nuget.org/profiles/FacturX.NET" },
    ],
  },
});
