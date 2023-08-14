import { exists } from "files";
import { extract } from "./jsonConfigsExtractor.js";
import { makeCsv } from "./csvMaker.js";

const islandsJson2Csv = async () => {
  const sourceDir = "./input/islands";
  if (!await exists(sourceDir)) {
    throw new Error("'./input/islands' folder doesn't exists");
  }

  const extractedData = await extract(sourceDir);
  await makeCsv(extractedData, "./output/islands.csv", (jsonRecords) => {
    return jsonRecords.sort((a, b) => a.Number - b.Number);
  });
};

const upgradesJson2Csv = async () => {
  const sourceDir = "./input/upgrades";
  if (!await exists(sourceDir)) {
    throw new Error("'./input/upgrades' folder doesn't exists");
  }

  const extractedData = await extract(sourceDir);
  await makeCsv(extractedData, "./output/upgrades.csv", (jsonRecords) => {
    return jsonRecords.sort((a, b) => a.Id - b.Id);
  });
};

await islandsJson2Csv();
await upgradesJson2Csv();