import { write } from "files";

export async function makeCsv (jsonRecords, outputPath) {
  let content = "";

  if (!jsonRecords || jsonRecords.length === 0) {
    throw new Error("Json records arrays is empty or null");
  }

  const headRecordKeys = [];
  for (const jsonRecord of jsonRecords) {
    const jsonRecordKeys = Object.keys(jsonRecord);
    for (let jsonRecordKey of jsonRecordKeys) {
      if (!headRecordKeys.includes(jsonRecordKey)) {
        headRecordKeys.push(jsonRecordKey);
      }
    }
  }
  for (let i = 0; i < headRecordKeys.length; i++) {
    content += headRecordKeys[i] + ",";
    if (i === headRecordKeys.length - 1) {
      content += "\n\r";
    }
  }
  const sortedJsonRecords = jsonRecords.sort((a, b) => a.Number - b.Number);
  for (let i = 0; i < sortedJsonRecords.length; i++) {
    const record = sortedJsonRecords[i];
    for (let j = 0; j < headRecordKeys.length; j++) {
      const recordProperty = record[headRecordKeys[j]];
      content += (recordProperty ? recordProperty : "") + ",";
      if (j === headRecordKeys.length - 1) {
        content += "\n\r";
      }
    }
  }

  await write(outputPath, content);
}