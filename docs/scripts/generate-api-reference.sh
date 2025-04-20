#!/usr/bin/env bash

# This script expects the following dotnet tools to be installed:
# - docfx - https://github.com/dotnet/docfx
# - dfmg - https://github.com/Jan0660/DocFxMarkdownGen
#
# It must be executed from the root of the repository

mkdir -p dist/api-reference
cp docfx.json dist/api-reference/docfx.json

cd dist/api-reference
docfx docfx.json
dfmg

# add frontmatter attributes
find . -type f -name "*.md" -exec sed -i 's/---\ntitle:/---\npageClass: pageClass: dotnet-api-reference-page\neditLink: false\ntitle:/g' {} \;

rm -rf ../../docs/src/api-reference/*
mv md/* ../../docs/src/api-reference/