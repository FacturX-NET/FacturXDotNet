const { execSync } = require("child_process");
const fs = require("fs");
const path = require("path");

const dependenciesFileDirectory = path.join(__dirname, "..", "src", "dependencies");
fs.mkdirSync(dependenciesFileDirectory, { recursive: true });

const dependenciesFilePath = path.join(dependenciesFileDirectory, "sbom.json");
const dependenciesFile = fs.openSync(dependenciesFilePath, "w+");
execSync("npm sbom --sbom-format cyclonedx", { stdio: ["inherit", dependenciesFile, "inherit"] });
fs.closeSync(dependenciesFile);
console.log(`Dependencies have been written at ${dependenciesFilePath}`);
