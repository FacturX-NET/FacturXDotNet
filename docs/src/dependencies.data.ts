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

    const result: Dependencies = {
      docs: {
        sbomLink: "/docs.bom.json",
        licenses: groupDependenciesByLicense([
          ...loadDependenciesFromSbom(docsSbom),
          {
            name: "docfx",
            author: ".NET Foundation and Contributors",
            version: "2.78.3",
            license: "MIT",
            link: "https://github.com/dotnet/docfx",
          },
          {
            name: "DocFxMarkdownGen ",
            author: "Jan0660 ",
            version: "0.4.2",
            license: "MIT",
            link: "https://github.com/Jan0660/DocFxMarkdownGen",
          },
        ]),
      },
      editor: {
        sbomLink: "/editor.bom.json",
        licenses: groupDependenciesByLicense(
          loadDependenciesFromSbom(editorSbom),
        ),
      },
      api: {
        sbomLink: "/api.bom.json",
        licenses: groupDependenciesByLicense(loadDependenciesFromSbom(apiSbom)),
      },
      cli: {
        sbomLink: "/cli.bom.json",
        licenses: groupDependenciesByLicense(loadDependenciesFromSbom(cliSbom)),
      },
      library: {
        sbomLink: "/library.bom.json",
        licenses: groupDependenciesByLicense(
          loadDependenciesFromSbom(librarySbom),
        ),
      },
    };

    console.log(result);

    return result;
  },
};

interface Dependencies {
  docs: { sbomLink: string; licenses: LicenseGroup[] };
  editor: { sbomLink: string; licenses: LicenseGroup[] };
  api: { sbomLink: string; licenses: LicenseGroup[] };
  cli: { sbomLink: string; licenses: LicenseGroup[] };
  library: { sbomLink: string; licenses: LicenseGroup[] };
}

interface LicenseGroup {
  readonly license: string;
  readonly dependencies: Dependency[];
}

interface Dependency {
  readonly name: string;
  readonly author: string;
  readonly version: string;
  readonly license: string;
  readonly link: string;
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
    a.license.localeCompare(b.license),
  );
}

function loadSbom(path: string) {
  const fileContent = fs.readFileSync(path, "utf8");
  return JSON.parse(fileContent) as Sbom;
}

function loadDependenciesFromSbom(sbom: Sbom): Dependency[] {
  const thisComponentName = sbom.metadata.component["bom-ref"];
  const thisComponentDependencies = sbom.dependencies[thisComponentName];
  if (thisComponentDependencies === undefined) {
    return [];
  }

  const dependencies = sbom.components.filter((c) =>
    thisComponentDependencies.includes(c["bom-ref"]),
  );

  return dependencies.map(
    (component: SbomComponent): Dependency => ({
      name: component.name,
      author: component.author,
      version: component.version,
      license: getLicense(component.licenses),
      link: getLink(component.externalReferences),
    }),
  );
}

function getLicense(licenses: SbomLicense[] | undefined): string | undefined {
  if (licenses === undefined) {
    return undefined;
  }

  const licenseNames = licenses.map((license) => {
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

  const vcsLink = externalReferences.find((r) => r.type === "vcs")?.url;
  if (vcsLink !== undefined) {
    return getRepositoryUrl(vcsLink);
  }

  return externalReferences.find((r) => r.type === "website")?.url;
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
