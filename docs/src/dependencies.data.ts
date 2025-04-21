import * as fs from "node:fs";

export default {
  load(): Dependencies {
    return {
      docs: groupDependenciesByLicense(
        loadDependenciesFromLicenseReportOutput(
          "src/assets/docs-licenses.json",
        ),
      ),
      editor: groupDependenciesByLicense(
        loadDependenciesFromLicenseReportOutput(
          "src/assets/editor-licenses.json",
        ),
      ),
      api: groupDependenciesByLicense(
        loadDependenciesFromDotNetProjectLicensesOutput(
          "src/assets/api-licenses.json",
        ),
      ),
      cli: groupDependenciesByLicense(
        loadDependenciesFromDotNetProjectLicensesOutput(
          "src/assets/cli-licenses.json",
        ),
      ),
      library: groupDependenciesByLicense(
        loadDependenciesFromDotNetProjectLicensesOutput(
          "src/assets/library-licenses.json",
        ),
      ),
    };
  },
};

interface Dependencies {
  docs: LicenseGroup[];
  editor: LicenseGroup[];
  api: LicenseGroup[];
  cli: LicenseGroup[];
  library: LicenseGroup[];
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

function loadDependenciesFromLicenseReportOutput(path: string): Dependency[] {
  const fileContent = fs.readFileSync(path, "utf8");
  const parsed = JSON.parse(fileContent) as LicenseReportOutput;
  return parsed.map(
    (d: LicenseReportOutputElement): Dependency => ({
      name: d.name,
      author: d.author,
      version: d.installedVersion,
      license: d.licenseType,
      link: getRepositoryUrl(d.link),
    }),
  );
}

type LicenseReportOutput = LicenseReportOutputElement[];

interface LicenseReportOutputElement {
  readonly name: string;
  readonly author: string;
  readonly installedVersion: string;
  readonly licenseType: string;
  readonly link: string;
}

function loadDependenciesFromDotNetProjectLicensesOutput(
  path: string,
): Dependency[] {
  const fileContent = fs.readFileSync(path, "utf8");
  const parsed = JSON.parse(fileContent) as DotNetProjectLicensesOutput;
  return parsed.map(
    (d: DotNetProjectLicensesOutputElement): Dependency => ({
      name: d.PackageName,
      author: d.Authors.join(", "),
      version: d.PackageVersion,
      license: d.LicenseType,
      link: getRepositoryUrl(d.Repository?.Url),
    }),
  );
}

type DotNetProjectLicensesOutput = DotNetProjectLicensesOutputElement[];

interface DotNetProjectLicensesOutputElement {
  readonly PackageName: string;
  readonly PackageVersion: string;
  readonly PackageUrl: string;
  readonly Copyright: string;
  readonly Authors: string[];
  readonly Description: string;
  readonly LicenseUrl: string;
  readonly LicenseType: string;
  readonly Repository: {
    readonly Type: string;
    readonly Url: string;
    readonly Commit: string;
  };
}

const gitPlusUrlRegExp = new RegExp(/git\+(.*)\.git/);
const gitSchemeUrlRegExp = new RegExp(/git:\/\/(.*)\.git/);
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
