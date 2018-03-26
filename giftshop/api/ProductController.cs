using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using giftshop.Models;
using System.Web.Http.Description;
using System.Data.Entity;

namespace giftshop.api
{
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
        GiftshopEntities dbContext = null;

        public ProductController() {
            dbContext = new GiftshopEntities();
        }

        [ResponseType(typeof(gs_t_cat_products))]
        [HttpPost]
        public HttpResponseMessage SaveProduct(gs_t_cat_products aproduct)
        {
            int result = 0;
            try
            {
                dbContext.gs_t_cat_products.Add(aproduct);
                dbContext.SaveChanges();
                result = 1;
            }
            catch (Exception e)
            {

                result = 0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [ResponseType(typeof(gs_t_cat_products))]
        [HttpPut]
        public HttpResponseMessage UpdateProduct(gs_t_cat_products aproduct)
        {
            int result = 0;
            try
            {
                dbContext.gs_t_cat_products.Attach(aproduct);
                dbContext.Entry(aproduct).State = EntityState.Modified;
                dbContext.SaveChanges();
                result = 1;
            }
            catch (Exception e)
            {

                result = 0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("DeleteProduct/{productID:int}")]
        [ResponseType(typeof(gs_t_cat_products))]
        [HttpDelete]
        public HttpResponseMessage DeleteProduct(int id)
        {
            int result = 0;
            try
            {
                var product = dbContext.gs_t_cat_products.Where(x => x.idproduct == id).FirstOrDefault();
                dbContext.gs_t_cat_products.Attach(product);
                dbContext.gs_t_cat_products.Remove(product);
                dbContext.SaveChanges();
                result = 1;
            }
            catch (Exception e)
            {

                result = 0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetProductByID/{productID:int}")]
        [ResponseType(typeof(gs_t_cat_products))]
        [HttpGet]
        public gs_t_cat_products GetProductByID(int productID)
        {
            gs_t_cat_products aproduct = null;
            try
            {
                aproduct = dbContext.gs_t_cat_products.Where(x => x.idproduct == productID).SingleOrDefault();

            }
            catch (Exception e)
            {
                aproduct = null;
            }

            return aproduct;
        }

        [ResponseType(typeof(gs_t_cat_products))]
        [HttpGet]
        public List<gs_t_cat_products> GetProducts()
        {
            List<gs_t_cat_products> products = null;
            try
            {
                products = dbContext.gs_t_cat_products.ToList();

            }
            catch (Exception e)
            {
                products = null;
            }

            return products;
        }

    }
}
