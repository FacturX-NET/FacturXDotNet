import {defineConfigWithTheme} from "vitepress"; // https://vitepress.dev/reference/site-config
import {useSidebar} from "vitepress-openapi";
import {getSidebar} from "vitepress-plugin-auto-sidebar";
import env from "../src/env.json" with {type: "json"};
import spec from "../src/assets/facturxdotnet.openapi.json" with {type: "json"};

const specSidebar = useSidebar({
  spec,
  linkPrefix: "/openapi-specification/",
  sidebarItemTemplate: (method, path) => {
    const operation = spec.paths[path]?.[method];
    return `<div class="OASidebarItem group/oaSidebarItem" style="display: grid; grid-template-columns: 1fr auto;">
        <span class="text" style="overflow: hidden; text-overflow: ellipsis; white-space: nowrap;">${operation ? operation.summary : path}</span>
        <span class="OASidebarItem-badge OAMethodBadge--${method.toLowerCase()}">${method.toUpperCase()}</span>
      </div>`;
  },
});

let cliItems =
  getSidebar({
    contentRoot: "/src",
    contentDirs: ["cli/"],
    useFrontmatter: false,
    collapsed: false,
    collapsible: false,
  })[0]?.items ?? [];
cliItems = cliItems.map(item => ({
  ...item,
  text: item.text.toLowerCase() === "subcommands" ? "Sub Commands" : item.text,
}));

let apiReferenceItems =
  getSidebar({
    contentRoot: "/src",
    contentDirs: ["api-reference/"],
    useFrontmatter: true,
    collapsed: true,
    collapsible: true,
  })[0]?.items ?? [];
apiReferenceItems = apiReferenceItems.filter(
  i => i.items !== undefined && i.items.length > 0,
);

// https://vitepress.dev/reference/site-config
export default defineConfigWithTheme({
  srcDir: "./src",
  head: [
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
        text: "Try it live!",
        items: [
          {
            text: "Try the Editor",
            link: env.editor.url,
          },
          {
            text: "Try the API",
            link: env.api.url,
          },
        ],
      },
      {
        text: "References",
        items: [
          {
            text: "OpenAPI specification",
            link: "/openapi-specification/introduction",
          },
          { text: "CLI API reference", link: "/cli/facturx" },
          { text: "Library API reference", link: "/api-reference/index" },
        ],
      },
      {
        text: "About",
        link: "/guides/about",
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
          text: "About",
          link: "/guides/about",
        },
        {
          text: `<span class='sidebar-footer sidebar-footer-first'>${env.buildName}</span>`,
        },
        {
          text: `<span class='sidebar-footer'>${env.version}</span>`,
        },
      ],
      "/openapi-specification/": [
        {
          text: "Introduction",
          link: "/openapi-specification/introduction",
        },
        ...specSidebar.generateSidebarGroups(),
        {
          text: `<span class='sidebar-footer sidebar-footer-first'>${env.buildName}</span>`,
        },
        {
          text: `<span class='sidebar-footer'>${env.version}</span>`,
        },
      ],
      "/cli/": [
        ...cliItems,
        {
          text: `<span class='sidebar-footer sidebar-footer-first'>${env.buildName}</span>`,
        },
        {
          text: `<span class='sidebar-footer'>${env.version}</span>`,
        },
      ],
      "/api-reference/": [
        { text: "Index", link: "/api-reference/index" },
        ...apiReferenceItems,
        {
          text: `<span class='sidebar-footer sidebar-footer-first'>${env.buildName}</span>`,
        },
        {
          text: `<span class='sidebar-footer'>${env.version}</span>`,
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

  transformPageData: pageData => {
    const result = { ...pageData };
    result.frontmatter = expandEnvInRecord(pageData.frontmatter);
    return result;
  },
});

function expandEnv(value: unknown): unknown {
  if (typeof value === "string" || value instanceof String) {
    return expandEnvInString(value);
  } else if (value.constructor.name == "Array") {
    return expandEnvInArray(value);
  } else {
    return expandEnvInRecord(value);
  }
}

function expandEnvInArray(array: unknown[]): unknown[] {
  return array.map(expandEnv);
}

function expandEnvInRecord(
  record: Record<unknown, unknown>,
): Record<unknown, unknown> {
  const result = {};

  for (const key of Object.keys(record)) {
    result[key] = expandEnv(record[key]);
  }

  return result;
}

function expandEnvInString(value: string): string {
  return value.replace(/\$env\.([^ ]*)/g, (_, varName) => getEnvValue(varName));
}

function getEnvValue(varName: string): string {
  const fragments = varName.split(".");
  let result = env;

  for (const fragment of fragments) {
    if (!Object.keys(result).includes(fragment)) {
      return "";
    }

    result = result[fragment];
  }

  return result;
}
