#!/usr/bin/env bash

# Find random available port
# Copied from https://unix.stackexchange.com/a/423052

comm -23 <(seq 49152 65535 | sort) <(ss -Htan | awk '{print $4}' | cut -d':' -f2 | sort -u) | shuf | head -n 1