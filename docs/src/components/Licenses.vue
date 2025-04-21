<script setup>
defineProps(["dependencyGroups"]);
</script>

<template>
  <ul>
    <li v-for="group in dependencyGroups">
      <b>{{ group.license }}</b>
      <ul>
        <li v-for="dependency in group.dependencies">
          <a :href="dependency.link" target="_blank">{{ dependency.name }}</a>
          v{{ dependency.version }}
          –
          <span class="text-muted-foreground">{{ dependency.author }}</span>
        </li>
      </ul>
    </li>
  </ul>
</template>

<style scoped></style>
