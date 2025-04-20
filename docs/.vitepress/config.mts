import {defineConfig} from "vitepress"; // https://vitepress.dev/reference/site-config
import {withPwa} from "@vite-pwa/vitepress"; // https://vitepress.dev/reference/site-config
import {useSidebar} from "vitepress-openapi";
import spec from "../src/assets/facturxdotnet.openapi.json" with {type: "json"};

const specSidebar = useSidebar({
  spec,
  linkPrefix: "/openapi-specification/",
});

// https://vitepress.dev/reference/site-config
export default withPwa(
  defineConfig({
    srcDir: "./src",
    head: [
      ["link", { rel: "icon", href: "/favicon.ico" }],
      [
        "link",
        {
          rel: "stylesheet",
          href: "https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css",
        },
      ],
    ],
    cleanUrls: true,

    title: "FacturX.NET",
    description:
      "FacturX.NET gives you a unified platform to manage Factur-X documents the way you prefer.",

    lastUpdated: true,
    themeConfig: {
      // https://vitepress.dev/reference/default-theme-config
      logo: "/favicon.png",

      nav: [
        { text: "Home", link: "/" },
        { text: "Guides", link: "/guides/getting-started" },
        {
          text: "Try the Editor",
          link: "https://{BUILD-NAME}.facturxdotnet.org/editor",
        },
        {
          text: "Try the API",
          link: "https://{BUILD-NAME}.facturxdotnet.org/api",
        },
        {
          text: "References",
          items: [
            {
              text: "OpenAPI specification",
              link: "/openapi-specification/introduction",
            },
            { text: "CLI", link: "/cli/getting-started" },
            { text: ".NET API reference", link: "/api-reference/overview" },
          ],
        },
      ],

      sidebar: {
        "/guides/": [
          {
            text: "What is FacturX.NET?",
            link: "/guides/what-is-facturxdotnet",
          },
          {
            text: "Getting started",
            link: "/guides/getting-started",
          },
          {
            text: "Use cases",
            items: [
              {
                text: "Generation",
                items: [
                  {
                    text: "Generate a Factur-X document",
                    link: "/guides/generation/facturx",
                  },
                  {
                    text: "Generate a standard PDF",
                    link: "/guides/generation/standard-pdf",
                  },
                ],
              },
              {
                text: "Validation",
                items: [
                  {
                    text: "Validate a Factur-X document",
                    link: "/guides/validation/facturx",
                  },
                  {
                    text: "Validate Cross-Industry Invoice data",
                    link: "/guides/validation/cii",
                  },
                ],
              },
              {
                text: "Extraction",
                items: [
                  {
                    text: "Extract Cross-Industry Invoice data",
                    link: "/guides/extraction/cii",
                  },
                  { text: "Extract XMP metadata", link: "/extraction/cii" },
                ],
              },
            ],
          },
          {
            text: "<span class='sidebar-footer'>v{VERSION}</span>",
          },
        ],
        "/openapi-specification/": [
          {
            text: "Introduction",
            link: "/openapi-specification/introduction",
          },
          ...specSidebar.generateSidebarGroups({
            linkPrefix: "/openapi-specification/",
          }),
        ],
        "/cli/": [
          {
            text: "CLI",
            items: [{ text: "CLI usage", link: "/cli/cli-usage" }],
          },
          {
            text: "<span class='sidebar-footer'>v{VERSION}</span>",
          },
        ],
        "/api-reference/": [
          {
            text: ".NET library",
            items: [{ text: "API reference", link: "/library/api-reference" }],
          },
          {
            text: "<span class='sidebar-footer'>v{VERSION}</span>",
          },
        ],
      },

      footer: {
        message:
          "The tools are open source and released under the MIT License, feel free to use, modify, and share.",
        copyright:
          "© 2025 Ismail Bennani, made with <i class='bi bi-heart-fill' style='color:red'></i> and <i class='bi bi-cup-hot-fill'></i>.",
      },

      socialLinks: [
        {
          icon: "github",
          link: "https://github.com/FacturX-NET/FacturXDotNet",
        },
        { icon: "nuget", link: "https://www.nuget.org/profiles/FacturX.NET" },
      ],

      editLink: {
        pattern:
          "https://github.com/FacturX-NET/FacturXDotNet/edit/main/docs/:path",
        text: "Edit this page on GitHub",
      },

      search: {
        provider: "local",
      },

      ssr: {
        noExternal: ["vitepress-plugin-nprogress"],
      },
    },
  }),
);
