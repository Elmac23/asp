import express from "express";
import routes from "./ui/routes";

const app = express();
const PORT = 3000;

app.use(express.json());
app.use("/api", routes);

app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
