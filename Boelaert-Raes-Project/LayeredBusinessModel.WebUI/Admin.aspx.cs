using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;
using CustomException;

namespace LayeredBusinessModel.WebUI
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Genre> genres = new GenreService().getGenres();
            foreach (Genre g in genres)
            {
                inputGenre.Items.Add(new ListItem(g.genre_id+", "+g.name));
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            String genre_id = inputGenre.Value.Split(',')[0]; //selects the ID
            String name = inputName.Value;
            String year = inputYear.Value;
            String barcode = new Random().Next(999999999).ToString(); //random barcode
            String author = inputAuthor.Value;
            String description = inputDescription.Value;
            float rentPrice = (float)Convert.ToDouble(inputRent.Value);
            float buyPrice = (float)Convert.ToDouble(inputBuy.Value);
            String dateAdded = inputDate.Value;
            int amountSold = Convert.ToInt32(inputAmountSold.Value);
            String[] actors = inputActors.Value.Split(',');
            String duration = inputDuration.Value;

            DvdInfo dvdInfo = new DvdInfo
            {
                name = name,
                year = year,
                barcode = barcode,
                author = author,
                descripion = description,
                rent_price = rentPrice,
                buy_price = buyPrice,
                date_added = DateTime.ParseExact(dateAdded, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                amount_sold = amountSold,
                actors = actors,
                duration = duration
            };

            DvdInfoService dvdInfoService = new DvdInfoService();
            int dvdInfoID = dvdInfoService.addDvdInfo(dvdInfo);
            if(dvdInfoID>=0)
            {
                lblStatus.Text = "Movie added to database.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblStatus.Text = "Something went wrong.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }

            //add genre record
            GenreService genreService = new GenreService();
            Genre genre = genreService.getGenre(genre_id);

            try
            {
                new DvdGenreService().addDvdGenre(genre, dvdInfo);            //Throws NorecordException
            }
            catch (NoRecordException)
            {

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //disabled code so we don't accidentally add more copies than needed

            // String id = txtDvdInfo.Text;
            //DvdCopyService dvdCopyService = new DvdCopyService();
            //dvdCopyService.addCopiesForDvd(id);
        }
    }
}