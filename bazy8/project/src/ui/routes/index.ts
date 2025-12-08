import { Router } from "express";

const router = Router();

router.get("/", (req, res) => {
  res.json({ message: "UI placeholder" });
});

export default router;
