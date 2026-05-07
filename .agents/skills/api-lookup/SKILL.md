---
name: api-lookup
description: Look up Nutrient Android SDK API documentation. Use when the user asks about a Nutrient API, class, method, annotation type, configuration option, or how to use a specific SDK feature.
---

# Nutrient Android API Lookup

Helps users find and understand Nutrient Android SDK APIs by consulting the official documentation.

## Workflow

### Step 1: Fetch the API Index

Fetch the llms.txt file to find relevant API entries:

```
WebFetch: https://www.nutrient.io/api/android/llms.txt
```

Search the response for packages or classes matching the user's query.

### Step 2: Fetch Specific API Documentation

Once you identify the relevant package or class from the llms.txt index, follow the links provided there to fetch the full documentation page for that class or package.

If the llms.txt entry provides a direct URL, use it. Otherwise, construct the URL using the base path and the fully-qualified class path as shown in the index.

### Step 3: Translate to .NET Binding Context

The Android API docs describe the Java/Kotlin API. When answering, translate to the .NET binding context:

1. **Read `Transforms/Metadata.xml`** to determine the actual namespace mappings, removed APIs, and type adjustments applied in this project.
2. **Read the `Additions/` directory** to find any hand-written C# enhancements that supplement the auto-generated binding.
3. Apply standard Java-to-C# binding conventions:
   - Java `camelCase` methods become C# `PascalCase`.
   - Java getter/setter pairs become C# properties.
   - Java enum constants become C# enum values.
   - Java listener interfaces become C# events or interface implementations.

### Step 4: Show Code Examples

When providing code examples, write them in C# using the .NET binding types. Cross-reference with the `Additions/` directory for any enhanced APIs that wrap native functionality.

## Notes

- The .NET binding may not expose every native API. Check `Transforms/Metadata.xml` for removal rules.
- The `Additions/` directory contains hand-written C# enhancements that supplement the auto-generated binding.
- For guides and tutorials (not API reference), use: `https://www.nutrient.io/guides/android/`
