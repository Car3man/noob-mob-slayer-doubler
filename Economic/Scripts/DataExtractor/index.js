import { exists } from "files";
import { extract } from "./islandExtractor.js";
import { makeCsv } from "./csvMaker.js";

const islandsJson2Csv = async () => {
  const sourceDir = "./input/islands";
  if (!await exists(sourceDir)) {
    throw new Error("'./input/islands' folder doesn't exists");
  }

  const extractedData = await extract(sourceDir);
  await makeCsv(extractedData, "./output/islands.csv");
};

await islandsJson2Csv();