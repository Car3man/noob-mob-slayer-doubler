import { write } from "files";

export async function makeCsv (jsonRecords, outputPath, sortPredicate) {
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
  const sortedJsonRecords = sortPredicate(jsonRecords);
  for (let i = 0; i < sortedJsonRecords.length; i++) {
    const record = sortedJsonRecords[i];
    for (let j = 0; j < headRecordKeys.length; j++) {
      const recordProperty = record[headRecordKeys[j]];
      let recordPropertyString;
      switch (typeof recordProperty) {
        case "object": recordPropertyString = "\"" + JSON.stringify(recordProperty).replaceAll("\"", "'") + "\""; break;
        case "undefined": recordPropertyString = ""; break;
        default: recordPropertyString = recordProperty; break;
      }
      content += recordPropertyString + ",";
      if (j === headRecordKeys.length - 1) {
        content += "\n\r";
      }
    }
  }

  await write(outputPath, content);
}