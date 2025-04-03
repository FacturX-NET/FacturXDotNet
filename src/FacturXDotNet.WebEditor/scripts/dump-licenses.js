const { execSync } = require("child_process");
const fs = require("fs");
const path = require("path");

const configFilePath = path.join(__dirname, "..", "license-report-config.json");
const outputFilePath = path.join(__dirname, "..", "src", "licenses", "licenses.json");

execSync("npm i -g license-report", { stdio: "inherit" });
console.log();

console.log(`Using configuration at at ${configFilePath}`);

const outputFile = fs.openSync(outputFilePath, "w+");
execSync(`license-report --config ${configFilePath}`, { stdio: ["inherit", outputFile, "inherit"] });
fs.closeSync(outputFile);

console.log(`Licenses have been written at ${outputFilePath}`);
