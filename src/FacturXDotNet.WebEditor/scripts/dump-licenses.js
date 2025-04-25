const { execSync } = require("child_process");
const fs = require("fs");
const path = require("path");

const dependenciesFileDirectory = path.join(__dirname, "..", "src", "dependencies");
fs.mkdirSync(dependenciesFileDirectory, { recursive: true });

const dependenciesFilePath = path.join(dependenciesFileDirectory, "dependencies.json");
const dependenciesFile = fs.openSync(dependenciesFilePath, "w+");
execSync("npm sbom --sbom-format cyclonedx", { stdio: ["inherit", dependenciesFile, "inherit"] });
fs.closeSync(dependenciesFile);
console.log(`Dependencies have been written at ${dependenciesFilePath}`);

const packageJson = JSON.parse(fs.readFileSync("package.json", "utf8"));
const directDependenciesNames = [
  ...Object.entries(packageJson.dependencies).map(([name, version]) => ({ name, version })),
  ...Object.entries(packageJson.devDependencies).map(([name, version]) => ({ name, version })),
];
const directDependenciesFilePath = path.join(dependenciesFileDirectory, "direct-dependencies.json");
fs.writeFileSync(directDependenciesFilePath, JSON.stringify(directDependenciesNames));
console.log(`Direct dependencies have been written at ${dependenciesFilePath}`);
