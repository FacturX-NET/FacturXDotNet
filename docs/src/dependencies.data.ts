import * as fs from "node:fs";

export default {
  watch: [
    "src/assets/docs.bom.json",
    "src/assets/editor.bom.json",
    "src/assets/api.bom.json",
    "src/assets/cli.bom.json",
    "src/assets/library.bom.json",
  ],
  load(): Dependencies {
    const docsSbom = loadSbom("src/public/docs.bom.json");
    const editorSbom = loadSbom("src/public/editor.bom.json");
    const apiSbom = loadSbom("src/public/api.bom.json");
    const cliSbom = loadSbom("src/public/cli.bom.json");
    const librarySbom = loadSbom("src/public/library.bom.json");

    const docsDependencies = [
      ...loadDependenciesFromSbom(docsSbom),
      {
        name: "docfx",
        version: "2.78.3",
        author: ".NET Foundation and Contributors",
        description: "The docfx command line tool published as .NET tool",
        license: "MIT",
        link: "https://github.com/dotnet/docfx",
      },
      {
        name: "DocFxMarkdownGen ",
        version: "0.4.2",
        author: "Jan0660 ",
        description: "Docusaurus Markdown generator using DocFX.",
        license: "MIT",
        link: "https://github.com/Jan0660/DocFxMarkdownGen",
      },
    ];
    const editorDependencies = loadDependenciesFromSbom(editorSbom);
    const apiDependencies = loadDependenciesFromSbom(apiSbom);
    const cliDependencies = loadDependenciesFromSbom(cliSbom);
    const libraryDependencies = loadDependenciesFromSbom(librarySbom);

    return {
      docs: {
        sbomLink: "/docs.bom.json",
        dependenciesCount: docsDependencies.length,
        licenses: groupDependenciesByLicense(docsDependencies),
      },
      editor: {
        sbomLink: "/editor.bom.json",
        dependenciesCount: editorDependencies.length,
        licenses: groupDependenciesByLicense(editorDependencies),
      },
      api: {
        sbomLink: "/api.bom.json",
        dependenciesCount: apiDependencies.length,
        licenses: groupDependenciesByLicense(apiDependencies),
      },
      cli: {
        sbomLink: "/cli.bom.json",
        dependenciesCount: cliDependencies.length,
        licenses: groupDependenciesByLicense(cliDependencies),
      },
      library: {
        sbomLink: "/library.bom.json",
        dependenciesCount: libraryDependencies.length,
        licenses: groupDependenciesByLicense(libraryDependencies),
      },
    };
  },
};

interface Dependencies {
  docs: {
    sbomLink: string;
    dependenciesCount: number;
    licenses: LicenseGroup[];
  };
  editor: {
    sbomLink: string;
    dependenciesCount: number;
    licenses: LicenseGroup[];
  };
  api: {
    sbomLink: string;
    dependenciesCount: number;
    licenses: LicenseGroup[];
  };
  cli: {
    sbomLink: string;
    dependenciesCount: number;
    licenses: LicenseGroup[];
  };
  library: {
    sbomLink: string;
    dependenciesCount: number;
    licenses: LicenseGroup[];
  };
}

interface LicenseGroup {
  readonly license: string;
  readonly dependencies: Dependency[];
}

interface Dependency {
  readonly name: string;
  readonly version: string;
  readonly author?: string;
  readonly description?: string;
  readonly license?: string;
  readonly link?: string;
}

function groupDependenciesByLicense(
  dependencies: Dependency[],
): LicenseGroup[] {
  const result: Record<string, LicenseGroup> = {};

  for (const dependency of dependencies) {
    if (!result[dependency.license]) {
      result[dependency.license] = {
        license: dependency.license,
        dependencies: [],
      };
    }

    result[dependency.license].dependencies.push(dependency);
  }

  return Object.values(result).sort((a, b) =>
    a.license === "" && b.license === ""
      ? 0
      : a.license === ""
        ? 1
        : b.license === ""
          ? -1
          : a.license.localeCompare(b.license),
  );
}

function loadSbom(path: string) {
  const fileContent = fs.readFileSync(path, "utf8");
  return JSON.parse(fileContent) as Sbom;
}

function loadDependenciesFromSbom(sbom: Sbom): Dependency[] {
  const thisComponentName = sbom.metadata.component["bom-ref"];
  const thisComponentDependencies = sbom.dependencies.find(
    d => d.ref === thisComponentName,
  )?.dependsOn;

  if (thisComponentDependencies === undefined) {
    return [];
  }

  const dependencies = sbom.components.filter(c =>
    thisComponentDependencies.includes(c["bom-ref"]),
  );

  return dependencies.map(
    (component: SbomComponent): Dependency => ({
      name: component.name,
      version: component.version,
      author: component.author,
      description: component.description,
      license: getLicense(component.licenses),
      link: getLink(component.externalReferences),
    }),
  );
}

function getLicense(licenses: SbomLicense[] | undefined): string | undefined {
  if (licenses === undefined) {
    return undefined;
  }

  const licenseNames = licenses.map(license => {
    if (isSbomLicenseExpression(license)) {
      if (
        license.expression.startsWith("(") &&
        license.expression.endsWith(")")
      ) {
        return license.expression.substring(1, license.expression.length - 2);
      }

      return license.expression;
    }

    return license.license.id;
  });

  return licenseNames.join(" OR ");
}

function getLink(
  externalReferences: SbomExternalReference[] | undefined,
): string | undefined {
  if (externalReferences === undefined) {
    return undefined;
  }

  const vcsLink = externalReferences.find(r => r.type === "vcs")?.url;
  if (vcsLink !== undefined) {
    return getRepositoryUrl(vcsLink);
  }

  return externalReferences.find(r => r.type === "website")?.url;
}

const gitPlusUrlRegExp = new RegExp(/git\+(.*)\.git/g);
const gitSchemeUrlRegExp = new RegExp(/git:\/\/(.*)\.git/g);

function getRepositoryUrl(url: string): string {
  if (url === undefined || url === null) {
    return url;
  }

  const gitPlusUrlRegExpMatch = url.match(gitPlusUrlRegExp);
  if (gitPlusUrlRegExpMatch !== null) {
    return gitPlusUrlRegExpMatch[1];
  }

  const gitSchemeUrlRegExpMatch = url.match(gitSchemeUrlRegExp);
  if (gitSchemeUrlRegExpMatch !== null) {
    return `https://${gitSchemeUrlRegExpMatch[1]}`;
  }

  return url;
}

export interface Sbom {
  readonly version: number;
  readonly metadata: {
    readonly component: SbomComponent;
  };
  readonly components: SbomComponent[];
  readonly dependencies: {
    readonly ref: string;
    readonly dependsOn: string[];
  }[];
}

export interface SbomComponent {
  readonly "bom-ref": string;
  readonly name: string;
  readonly version: string;
  readonly author?: string;
  readonly description?: string;
  readonly licenses?: SbomLicense[];
  readonly externalReferences: SbomExternalReference[];
}

export type SbomLicense = SbomLicenseId | SbomLicenseExpression;

export interface SbomLicenseId {
  readonly license: { readonly id: string };
}

export interface SbomLicenseExpression {
  readonly expression: string;
}

export function isSbomLicenseExpression(
  license: SbomLicense,
): license is SbomLicenseExpression {
  return Object.keys(license).includes("expression");
}

export interface SbomExternalReference {
  readonly type: string;
  readonly url: string;
}
