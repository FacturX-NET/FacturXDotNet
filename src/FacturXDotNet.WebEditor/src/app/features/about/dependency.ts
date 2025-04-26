import { isSbomLicenseExpression, Sbom, SbomComponent, SbomExternalReference, SbomLicense } from '../../core/sbom';

export interface Dependency {
  name: string;
  description?: string;
  author?: string;
  version: string;
  license?: string;
  link?: string;
  direct: boolean;
}

export function extractDependenciesFromSbom(sbom: Sbom) {
  return sbom.components.map((component) => {
    return {
      name: component.name,
      description: component.description,
      author: component.author,
      version: component.version,
      license: getLicense(component.licenses),
      link: getLink(component.externalReferences),
      direct: isDirectDependency(sbom, component),
    };
  });
}

function getLicense(licenses: SbomLicense[] | undefined): string | undefined {
  if (licenses === undefined) {
    return undefined;
  }

  const licenseNames = licenses.map((license) => {
    if (isSbomLicenseExpression(license)) {
      if (license.expression.startsWith('(') && license.expression.endsWith(')')) {
        return license.expression.substring(1, license.expression.length - 2);
      }

      return license.expression;
    }

    return license.license.id;
  });

  return licenseNames.join(' OR ');
}

function getLink(externalReferences: SbomExternalReference[] | undefined): string | undefined {
  if (externalReferences === undefined) {
    return undefined;
  }

  const vcsLink = externalReferences.find((r) => r.type === 'vcs')?.url;
  if (vcsLink !== undefined) {
    return getRepositoryUrl(vcsLink);
  }

  return externalReferences.find((r) => r.type === 'website')?.url;
}

function isDirectDependency(sbom: Sbom, component: SbomComponent): boolean {
  const thisComponent = sbom.metadata.component['bom-ref'];
  const dependencies = sbom.dependencies.find((d) => d.ref === thisComponent);
  if (dependencies === undefined) {
    return false;
  }

  return dependencies.dependsOn.includes(component['bom-ref']);
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
