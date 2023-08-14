import { read, list } from "files";

export async function extract (folder) {
  const listedFiles = await list(folder);
  const files = [];
  for (const filePath of listedFiles) {
    const file = await read(filePath);
    files.push(file);
  }
  const islandsJson = [];
  for (const file of files) {
    const island = JSON.parse(file.trim());
    islandsJson.push(island);
  }
  return islandsJson;
}