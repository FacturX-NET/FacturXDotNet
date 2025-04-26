import { isSbomLicenseExpression, Sbom, SbomComponent, SbomLicense, SbomLicenseExpression } from '../../core/sbom';

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
      license: getLicense(component.licenses?.[0]),
      link: component.externalReferences.find((r) => r.type === 'website')?.url,
      direct: isDirectDependency(sbom, component),
    };
  });
}

function getLicense(license: SbomLicense | undefined): string | undefined {
  if (license === undefined) {
    return undefined;
  }

  if (isSbomLicenseExpression(license)) {
    return license.expression;
  }

  return license.license.id;
}

function isDirectDependency(sbom: Sbom, component: SbomComponent): boolean {
  const thisComponent = sbom.metadata.component['bom-ref'];
  const dependencies = sbom.dependencies.find((d) => d.ref === thisComponent);
  if (dependencies === undefined) {
    return false;
  }

  return dependencies.dependsOn.includes(component['bom-ref']);
}
