package lab.ejb;

import jakarta.ejb.Local;

import java.util.List;



@Local
public interface NewsItemFacadeLocal {

    void create(NewsItem item);
    List<NewsItem> getAllNewsItems();
}
