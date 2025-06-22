import React from 'react';
import { useParams, Link } from 'react-router-dom';

function ProductDetails({ products }) {
  const { id } = useParams();
  const filteredProducts = products.filter(product => product.id === Number(id));

  if (filteredProducts.length === 0) {
    return null;
  }

  const product = filteredProducts[0];

  return (
    <div>
      <h1>{product.title}</h1>
      Category: {product.category}<br/>
      Brand: {product.brand}<br />
      Description: {product.description}<br/>
      Price: ${product.price}<br/>
      <img src={product.thumbnail} alt={product.title}/>
      <br/>
      <br/>
      <Link to="/">Back</Link>
    </div>
  );
}

export default ProductDetails;