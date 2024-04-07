/*
 *  LIGen by Eldridge Misnomer
 *  
 *  A very simple tool which generates a few paragraphs of Lorem Ipsum
 *  and puts it in the clipboard.
 *  
 */

using System;
using System.Windows.Forms;

namespace LIGen {
    internal class Program {

        [STAThread]
        static void Main( string[] args ) {

            // Create a Lorem Ipsum generator
            LI li = new LI();
            // Generate a few paragraphs of Lorem Ipsum
            string litext = li.Gen();
            // Shove the generated text into the clipboard
            Clipboard.SetText( litext );
            // Close this program
            Environment.Exit( 0 );
        }
    }

    internal class LI {

        private Random rand = new Random();

        // The main text generator method
        public string Gen() {

            // Pick a random number of paragraphs to generate (3-5)
            // and shove them into a string
            int numpr = rand.Next( 3, 6 );
            string li = "";
            for( int p = 0; p < numpr; p++ ) {
                li += Mkpr( p == 0 );
                // if this is not the last paragraph add a couple of line breaks
                if (p < numpr-1) {
                    li += "\n\n";
                }
            }

            return li;
        }

        // a little utility for generating random numbers following a gaussian distribution
        // this comes from stack overflow: https://stackoverflow.com/a/218600
        // by user yoyoyoyosef
        private double GenGaus( double mean, double stdDev ) {
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double randStdNorm = Math.Sqrt( -2.0 * Math.Log( u1 ) ) *
                Math.Sin( 2.0 * Math.PI * u2 );
            double randG = mean + stdDev * randStdNorm;
            return randG;
        }

        // Capitalise the first character of a string
        private string CapFirst( string s ) {
            char[] ls = s.ToCharArray();
            ls[0] = char.ToUpper( ls[0] );
            return new string( ls );
        }

        // Generates one paragraph of Lorem Ipsum
        private string Mkpr( Boolean first = false ) {

            string pr = "";
            Boolean lstspt = true;
            Boolean thisspt = true;

            // We pick a number of sentences (or clauses) to generate
            // We do this with a gaussian distribution, but we enforce a minimum of three.
            int min = 3;
            int numsens = 2;
            while( numsens < min ) {
                numsens = (int)GenGaus( 5, 2 );
            }

            // If this is the first paragraph, we start it with "Lorem ipsum"
            // and we follow that without a capital letter
            if ( first ) {
                pr += "Lorem ipsum ";
                lstspt = rand.NextDouble() < 0.6;
                pr += Mkcl( false, lstspt );
                numsens -= 1;
            }

            // Keep producing sentences until we have enough
            while( numsens > 0 ) {
                string ns = Mkcl( lstspt, thisspt, numsens == 1 );
                // keep track of whether or not this sentence and the previous sentence
                // end with a comma or not (so we can capitalise properly (also affects clause length))
                lstspt = thisspt;
                thisspt = rand.NextDouble() < 0.7;
                pr += ns;
                numsens -= 1;
            }

            return pr;

        }

        // Generate one sentence (or clause) of Lorem Ipsum
        private string Mkcl( Boolean prespot = true, Boolean spot = true, Boolean end = false ) {
            // The list of words to pick from.
            // This comes from sections 1.10.32 and 1.10.33 of Cicero's De finibus bonorum et malorum
            // ( as per wikipedia ). We have removed all duplicates, capital letters and punctuation.
            string[] wrds = {
            "sed", "ut", "perspiciatis", "unde", "omnis",
            "iste", "natus", "error", "sit", "voluptatem",
            "accusantium", "doloremque", "laudantium", "totam", "rem",
            "aperiam", "eaque", "ipsa", "quae", "ab",
            "illo", "inventore", "veritatis", "et", "quasi",
            "architecto", "beatae", "vitae", "dicta", "sunt",
            "explicabo", "nemo", "enim", "ipsam", "quia",
            "voluptas", "aspernatur", "aut", "odit", "fugit",
            "consequuntur", "magni", "dolores", "eos", "qui",
            "ratione", "sequi", "nesciunt", "neque", "porro",
            "quisquam", "est", "dolorem", "ipsum", "dolor",
            "amet", "consectetur", "adipisci", "velit", "non",
            "numquam", "eius", "modi", "tempora", "incididunt",
            "labore", "dolore", "magnam", "aliquam", "quaerat",
            "ad", "minima", "veniam", "quis", "nostrum",
            "exercitationem", "ullam", "corporis", "suscipit",
            "laboriosam", "nisi", "aliquid", "ex", "ea",
            "commodi", "consequatur", "autem", "vel",
            "eum", "iure", "reprehenderit", "in", "voluptate",
            "esse", "quam", "nihil", "molestiae", "illum",
            "fugiat", "quo", "nulla", "pariatur", "at",
            "vero", "accusamus", "iusto", "odio", "dignissimos",
            "ducimus", "blanditiis", "praesentium", "voluptatum", "deleniti",
            "atque", "corrupti", "quos", "quas", "molestias",
            "excepturi", "sint", "obcaecati", "cupiditate", "provident",
            "similique", "culpa", "officia", "deserunt", "mollitia",
            "animi", "id", "laborum", "dolorum", "fuga",
            "harum", "quidem", "rerudum", "facilis", "expedita",
            "distinctio", "nam", "libero", "tempore", "cum",
            "soluta", "nobis", "eligendi", "optio", "cumque",
            "impedit", "minus", "quod", "maxime", "placeat",
            "facere", "possimus", "assumenda", "repellendus", "temporibus",
            "quibusdam", "officiis", "debitis", "rerum", "necessitatibus",
            "saepe", "eveniet", "voluptates", "repudiandae", "recusandae",
            "itaque", "earum", "hic", "tenetur", "a",
            "sapiente", "delectus", "reiciendis", "voluptatibus", "maiores",
            "alias", "perferendis", "doloribus", "asperiores", "repellat"
            };

            int tot = wrds.Length;

            // pick a sentence length (in number of words) using a gaussian distribution.
            // We enforce a minimum sentence length of 3 words
            double mean = 12;
            double stdDev = 10;
            int min = 3;

            // If this is not a full sentence (ie ends with a comma) or follows a comma, make it shorter (probably).
            if( !spot || prespot ) {
                mean = 6;
                stdDev = 4;
                min = 2;
            }

            int ln = min - 1;
            while( ln < min ) {
                ln = (int)GenGaus( mean, stdDev );
            }

            // Keep picking new words randomly until sentence has reached required length
            // The only rule we enforce is that we don't repeat the previous word
            string cl = "";
            int last = 0;

            for( int w = 0; w < ln; w++ ) {
                int pick = last;
                while( pick == last ) {
                    pick = rand.Next( 0, tot );
                }
                string newwrd = wrds[pick];
                // If this is the first word, and the previous clause finished with a full stop
                // capitalise the first letter
                if( w == 0 && prespot ) {
                    newwrd = CapFirst( newwrd );
                }
                // If this is not the last word, add a space after it
                if( w < ln - 1 ) {
                    newwrd += " ";
                }
                cl += newwrd;
            }

            // If this is the last sentence in a paragraph, finish it with a full stop
            if( end ) {
                cl += ".";
            } else {
                // if this is a full sentence
                // finish it with a full stop or a question mark.
                if( spot ) {
                    double qchance = 0.1;
                    if( rand.NextDouble() < qchance ) {
                        cl += "? ";
                    } else {
                        cl += ". ";
                    }
                    // if this is not a full sentence, finish it with a comma.
                } else {
                    cl += ", ";
                }
            }

            return cl;

        }
    }
}
