import { Router, Request, Response } from "express";
import { ProductService } from "../../application/services/ProductService";
import { ProductRepository } from "../../infrastructure/repositories/ProductRepository";

const router = Router();
const productRepository = new ProductRepository();
const productService = new ProductService(productRepository);

router.get("/", (req: Request, res: Response) => {
  res.send(`
    <!DOCTYPE html>
    <html>
    <head>
      <title>Product Store</title>
      <style>
        body { font-family: Arial, sans-serif; margin: 20px; background-color: #f5f5f5; }
        h1 { color: #333; }
        .container { max-width: 1200px; margin: 0 auto; background: white; padding: 20px; border-radius: 8px; }
        a { color: #007bff; text-decoration: none; }
        a:hover { text-decoration: underline; }
      </style>
    </head>
    <body>
      <div class="container">
        <h1>Welcome to Product Store</h1>
        <p><a href="/api/products">View All Products</a></p>
      </div>
    </body>
    </html>
  `);
});

router.get("/products", async (req: Request, res: Response) => {
  try {
    const products = await productService.getAllProducts();

    res.send(`
      <!DOCTYPE html>
      <html>
      <head>
        <title>Products List</title>
        <style>
          body { font-family: Arial, sans-serif; margin: 20px; background-color: #f5f5f5; }
          .container { max-width: 1200px; margin: 0 auto; background: white; padding: 20px; border-radius: 8px; }
          h1 { color: #333; }
          .product-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(250px, 1fr)); gap: 20px; margin-top: 20px; }
          .product-card { border: 1px solid #ddd; border-radius: 8px; padding: 15px; background: #fff; transition: box-shadow 0.3s; }
          .product-card:hover { box-shadow: 0 4px 8px rgba(0,0,0,0.1); }
          .product-name { font-size: 18px; font-weight: bold; color: #333; margin-bottom: 10px; }
          .product-price { font-size: 16px; color: #28a745; font-weight: bold; margin: 5px 0; }
          .product-stock { font-size: 14px; color: #666; margin: 5px 0; }
          .product-description { font-size: 14px; color: #666; margin: 10px 0; }
          .view-details { display: inline-block; margin-top: 10px; padding: 8px 15px; background: #007bff; color: white; text-decoration: none; border-radius: 4px; }
          .view-details:hover { background: #0056b3; }
          .back-link { display: inline-block; margin-bottom: 20px; color: #007bff; text-decoration: none; }
          .back-link:hover { text-decoration: underline; }
          .add-button { display: inline-block; padding: 10px 20px; background: #28a745; color: white; text-decoration: none; border-radius: 4px; float: right; }
          .add-button:hover { background: #218838; }
          .header-row { overflow: hidden; margin-bottom: 20px; }
          .edit-button { display: inline-block; margin-top: 10px; margin-left: 5px; padding: 8px 15px; background: #ffc107; color: #333; text-decoration: none; border-radius: 4px; }
          .edit-button:hover { background: #e0a800; }
        </style>
      </head>
      <body>
        <div class="container">
          <a href="/api" class="back-link">&larr; Back to Home</a>
          <div class="header-row">
            <h1 style="display: inline-block;">Products List</h1>
            <a href="/api/products-create" class="add-button">+ Add New Product</a>
          </div>
          ${
            products.length === 0
              ? "<p>No products available.</p>"
              : `
            <div class="product-grid">
              ${products
                .map(
                  (product) => `
                <div class="product-card">
                  <div class="product-name">${product.name}</div>
                  <div class="product-price">$${product.price.amount.toFixed(
                    2
                  )}</div>
                  <div class="product-stock">Stock: ${product.stock}</div>
                  <div class="product-description">${product.description.substring(
                    0,
                    100
                  )}${product.description.length > 100 ? "..." : ""}</div>
                  <a href="/api/products/${product.getId()}" class="view-details">View Details</a>
                  <a href="/api/products/${product.getId()}/edit" class="edit-button">Edit</a>
                </div>
              `
                )
                .join("")}
            </div>
          `
          }
        </div>
      </body>
      </html>
    `);
  } catch (error) {
    res.status(500).send(`<h1>Error loading products</h1><p>${error}</p>`);
  }
});

router.get("/products/:id", async (req: Request, res: Response) => {
  try {
    const product = await productService.getProductById(req.params.id);

    if (!product) {
      res.status(404).send(`
        <!DOCTYPE html>
        <html>
        <head>
          <title>Product Not Found</title>
          <style>
            body { font-family: Arial, sans-serif; margin: 20px; background-color: #f5f5f5; }
            .container { max-width: 800px; margin: 0 auto; background: white; padding: 20px; border-radius: 8px; }
            h1 { color: #dc3545; }
            a { color: #007bff; text-decoration: none; }
          </style>
        </head>
        <body>
          <div class="container">
            <h1>Product Not Found</h1>
            <p>The product you're looking for doesn't exist.</p>
            <a href="/api/products">&larr; Back to Products List</a>
          </div>
        </body>
        </html>
      `);
      return;
    }

    res.send(`
      <!DOCTYPE html>
      <html>
      <head>
        <title>${product.name} - Product Details</title>
        <style>
          body { font-family: Arial, sans-serif; margin: 20px; background-color: #f5f5f5; }
          .container { max-width: 800px; margin: 0 auto; background: white; padding: 30px; border-radius: 8px; }
          .back-link { display: inline-block; margin-bottom: 20px; color: #007bff; text-decoration: none; }
          .back-link:hover { text-decoration: underline; }
          .product-header { border-bottom: 2px solid #007bff; padding-bottom: 15px; margin-bottom: 20px; }
          .product-name { font-size: 32px; font-weight: bold; color: #333; margin: 0; }
          .product-id { font-size: 14px; color: #999; margin-top: 5px; }
          .product-details { margin-top: 20px; }
          .detail-row { margin: 15px 0; padding: 10px; background: #f8f9fa; border-radius: 4px; }
          .detail-label { font-weight: bold; color: #555; display: inline-block; width: 120px; }
          .detail-value { color: #333; }
          .price { font-size: 28px; color: #28a745; font-weight: bold; }
          .stock { font-size: 18px; }
          .stock.in-stock { color: #28a745; }
          .stock.low-stock { color: #ffc107; }
          .stock.out-of-stock { color: #dc3545; }
          .description { margin-top: 20px; line-height: 1.6; color: #555; }
          .edit-button { display: inline-block; padding: 10px 20px; background: #ffc107; color: #333; text-decoration: none; border-radius: 4px; margin-top: 20px; }
          .edit-button:hover { background: #e0a800; }
          .delete-button { display: inline-block; padding: 10px 20px; background: #dc3545; color: white; border: none; border-radius: 4px; margin-top: 20px; margin-left: 10px; cursor: pointer; font-size: 16px; }
          .delete-button:hover { background: #c82333; }
        </style>
      </head>
      <body>
        <div class="container">
          <a href="/api/products" class="back-link">&larr; Back to Products List</a>
          <div class="product-header">
            <h1 class="product-name">${product.name}</h1>
            <div class="product-id">Product ID: ${product.getId()}</div>
          </div>
          <div class="product-details">
            <div class="detail-row">
              <span class="detail-label">Price:</span>
              <span class="detail-value price">$${product.price.amount.toFixed(
                2
              )} ${product.price.currency}</span>
            </div>
            <div class="detail-row">
              <span class="detail-label">Stock:</span>
              <span class="detail-value stock ${
                product.stock > 10
                  ? "in-stock"
                  : product.stock > 0
                  ? "low-stock"
                  : "out-of-stock"
              }">
                ${product.stock} ${
      product.stock === 1 ? "unit" : "units"
    } available
              </span>
            </div>
            <div class="detail-row">
              <span class="detail-label">Description:</span>
            </div>
            <div class="description">${product.description}</div>
          </div>
          <a href="/api/products/${product.getId()}/edit" class="edit-button">Edit Product</a>
          <form method="POST" action="/api/products/${product.getId()}/delete" style="display: inline;">
            <button type="submit" class="delete-button" onclick="return confirm('Are you sure you want to delete this product?')">Delete Product</button>
          </form>
        </div>
      </body>
      </html>
    `);
  } catch (error) {
    res.status(500).send(`<h1>Error loading product</h1><p>${error}</p>`);
  }
});

router.get("/products-create", (req: Request, res: Response) => {
  res.send(`
    <!DOCTYPE html>
    <html>
    <head>
      <title>Add New Product</title>
      <style>
        body { font-family: Arial, sans-serif; margin: 20px; background-color: #f5f5f5; }
        .container { max-width: 600px; margin: 0 auto; background: white; padding: 30px; border-radius: 8px; }
        h1 { color: #333; margin-bottom: 20px; }
        .back-link { display: inline-block; margin-bottom: 20px; color: #007bff; text-decoration: none; }
        .back-link:hover { text-decoration: underline; }
        .form-group { margin-bottom: 20px; }
        label { display: block; font-weight: bold; margin-bottom: 5px; color: #555; }
        input, textarea { width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 4px; font-size: 14px; box-sizing: border-box; }
        textarea { min-height: 100px; resize: vertical; }
        .button-group { margin-top: 30px; }
        button { padding: 10px 20px; background: #28a745; color: white; border: none; border-radius: 4px; font-size: 16px; cursor: pointer; }
        button:hover { background: #218838; }
        .cancel-button { background: #6c757d; margin-left: 10px; }
        .cancel-button:hover { background: #5a6268; }
      </style>
    </head>
    <body>
      <div class="container">
        <a href="/api/products" class="back-link">&larr; Back to Products List</a>
        <h1>Add New Product</h1>
        <form method="POST" action="/api/products">
          <div class="form-group">
            <label for="id">Product ID *</label>
            <input type="text" id="id" name="id" required>
          </div>
          <div class="form-group">
            <label for="name">Name *</label>
            <input type="text" id="name" name="name" required>
          </div>
          <div class="form-group">
            <label for="description">Description *</label>
            <textarea id="description" name="description" required></textarea>
          </div>
          <div class="form-group">
            <label for="price">Price *</label>
            <input type="number" id="price" name="price" step="0.01" min="0" required>
          </div>
          <div class="form-group">
            <label for="stock">Stock *</label>
            <input type="number" id="stock" name="stock" min="0" required>
          </div>
          <div class="button-group">
            <button type="submit">Add Product</button>
            <button type="button" class="cancel-button" onclick="window.location.href='/api/products'">Cancel</button>
          </div>
        </form>
      </div>
    </body>
    </html>
  `);
});

router.post("/products", async (req: Request, res: Response) => {
  try {
    const { id, name, description, price, stock } = req.body;
    await productService.createProduct(
      id,
      name,
      description,
      parseFloat(price),
      parseInt(stock)
    );
    res.redirect("/api/products");
  } catch (error) {
    res.status(500).send(`<h1>Error creating product</h1><p>${error}</p>`);
  }
});

router.get("/products/:id/edit", async (req: Request, res: Response) => {
  try {
    const product = await productService.getProductById(req.params.id);

    if (!product) {
      res.status(404).send(`
        <!DOCTYPE html>
        <html>
        <head>
          <title>Product Not Found</title>
          <style>
            body { font-family: Arial, sans-serif; margin: 20px; background-color: #f5f5f5; }
            .container { max-width: 800px; margin: 0 auto; background: white; padding: 20px; border-radius: 8px; }
            h1 { color: #dc3545; }
            a { color: #007bff; text-decoration: none; }
          </style>
        </head>
        <body>
          <div class="container">
            <h1>Product Not Found</h1>
            <p>The product you're looking for doesn't exist.</p>
            <a href="/api/products">&larr; Back to Products List</a>
          </div>
        </body>
        </html>
      `);
      return;
    }

    res.send(`
      <!DOCTYPE html>
      <html>
      <head>
        <title>Edit Product - ${product.name}</title>
        <style>
          body { font-family: Arial, sans-serif; margin: 20px; background-color: #f5f5f5; }
          .container { max-width: 600px; margin: 0 auto; background: white; padding: 30px; border-radius: 8px; }
          h1 { color: #333; margin-bottom: 20px; }
          .back-link { display: inline-block; margin-bottom: 20px; color: #007bff; text-decoration: none; }
          .back-link:hover { text-decoration: underline; }
          .form-group { margin-bottom: 20px; }
          label { display: block; font-weight: bold; margin-bottom: 5px; color: #555; }
          input, textarea { width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 4px; font-size: 14px; box-sizing: border-box; }
          textarea { min-height: 100px; resize: vertical; }
          .button-group { margin-top: 30px; }
          button { padding: 10px 20px; background: #ffc107; color: #333; border: none; border-radius: 4px; font-size: 16px; cursor: pointer; font-weight: bold; }
          button:hover { background: #e0a800; }
          .cancel-button { background: #6c757d; color: white; margin-left: 10px; }
          .cancel-button:hover { background: #5a6268; }
          .readonly { background-color: #e9ecef; }
        </style>
      </head>
      <body>
        <div class="container">
          <a href="/api/products/${product.getId()}" class="back-link">&larr; Back to Product Details</a>
          <h1>Edit Product</h1>
          <form method="POST" action="/api/products/${product.getId()}">
            <div class="form-group">
              <label for="id">Product ID</label>
              <input type="text" id="id" name="id" value="${product.getId()}" readonly class="readonly">
            </div>
            <div class="form-group">
              <label for="name">Name *</label>
              <input type="text" id="name" name="name" value="${
                product.name
              }" required>
            </div>
            <div class="form-group">
              <label for="description">Description *</label>
              <textarea id="description" name="description" required>${
                product.description
              }</textarea>
            </div>
            <div class="form-group">
              <label for="price">Price *</label>
              <input type="number" id="price" name="price" step="0.01" min="0" value="${
                product.price.amount
              }" required>
            </div>
            <div class="form-group">
              <label for="stock">Stock *</label>
              <input type="number" id="stock" name="stock" min="0" value="${
                product.stock
              }" required>
            </div>
            <div class="button-group">
              <button type="submit">Update Product</button>
              <button type="button" class="cancel-button" onclick="window.location.href='/api/products/${product.getId()}'">Cancel</button>
            </div>
          </form>
        </div>
      </body>
      </html>
    `);
  } catch (error) {
    res.status(500).send(`<h1>Error loading product</h1><p>${error}</p>`);
  }
});

router.post("/products/:id", async (req: Request, res: Response) => {
  try {
    const { name, description, price, stock } = req.body;
    await productService.updateProduct(
      req.params.id,
      name,
      description,
      parseFloat(price),
      parseInt(stock)
    );
    res.redirect(`/api/products/${req.params.id}`);
  } catch (error) {
    res.status(500).send(`<h1>Error updating product</h1><p>${error}</p>`);
  }
});

router.post("/products/:id/delete", async (req: Request, res: Response) => {
  try {
    await productService.deleteProduct(req.params.id);
    res.redirect("/api/products");
  } catch (error) {
    res.status(500).send(`<h1>Error deleting product</h1><p>${error}</p>`);
  }
});

export default router;
