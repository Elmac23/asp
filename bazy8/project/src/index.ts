import express from "express";
import routes from "./ui/routes";

const app = express();
const PORT = 3000;

app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use("/api", routes);

app.get("/", (req, res) => {
  res.redirect("/api");
});

app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
