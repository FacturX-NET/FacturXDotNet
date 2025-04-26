<script setup>
import { withBase } from "vitepress";
import markdownit from "markdown-it";

defineProps(["dependencies"]);

const md = markdownit({
  linkify: true,
  breaks: true,
});
</script>

<template>
  <div class="dependencies">
    <span>
      <b> Dependencies </b> ({{ dependencies.dependenciesCount }})
      <a :href="withBase(dependencies.sbomLink)" download>sbom</a>
    </span>

    <ul>
      <li v-for="group in dependencies.licenses">
        <span v-if="group.license">
          <b>{{ group.license }}</b>
        </span>
        <span v-else>Unknown</span>
        ({{ group.dependencies.length }})
        <ul>
          <li v-for="dependency in group.dependencies" class="dependency">
            <span>
              <a :href="dependency.link" target="_blank">
                {{ dependency.name }}
              </a>
              <span v-if="dependency.version"> v{{ dependency.version }} </span>
              <template v-if="dependency.author">
                –
                <template v-if="dependency.author.startsWith('http')">
                  <a :href="dependency.author">authors</a>
                </template>
                <template v-else>
                  <b>
                    <span class="text-muted-foreground">
                      {{ dependency.author }}
                    </span>
                  </b>
                </template>
              </template>
            </span>

            <div
              v-if="dependency.description"
              class="dependency-description"
              v-html="md.render(dependency.description)"
            ></div>
          </li>
        </ul>
      </li>
    </ul>
  </div>
</template>

<style scoped>
.dependencies {
  margin-top: 16px;
}

.dependency {
  margin-bottom: 16px;
}
</style>
