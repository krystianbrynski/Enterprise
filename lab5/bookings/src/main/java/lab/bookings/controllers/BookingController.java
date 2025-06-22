package lab.bookings.controllers;

import jakarta.validation.Valid;
import lab.bookings.models.Booking;
import lab.bookings.models.Apartment;
import lab.bookings.repositories.ApartmentRepository;
import lab.bookings.repositories.BookingRepository;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.validation.Errors;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDate;
import java.util.List;

@Controller
@RequestMapping("/bookings")
public class BookingController {

    private final BookingRepository bookingRepository;
    private final ApartmentRepository apartmentRepository;

    public BookingController(BookingRepository bookingRepository,
                             ApartmentRepository apartmentRepository) {
        this.bookingRepository   = bookingRepository;
        this.apartmentRepository = apartmentRepository;
    }

    @PostMapping("/delete")
    public String delete(@RequestParam("id") Long id) {
        bookingRepository.deleteById(id);
        return "redirect:/bookings";
    }

    @GetMapping
    public String getAll(Model model) {
        model.addAttribute("booking",  new Booking());
        model.addAttribute("bookings", bookingRepository.findAll());
        return "bookings";
    }

    @PostMapping
    public String create(@Valid Booking booking, Errors errors, Model model) {
        if (errors.hasErrors()) {
            model.addAttribute("bookings", bookingRepository.findAll());
            return "bookings";
        }

        int numGuests      = booking.getNumGuests();
        LocalDate startDay = booking.getFromDate();
        LocalDate endDay   = booking.getToDate();

        List<Apartment> availableApartments =
                apartmentRepository.getFreeApartments(numGuests, startDay, endDay);

        if (!availableApartments.isEmpty()) {
            booking.setApartment(availableApartments.get(0));
            bookingRepository.save(booking);
            return "redirect:/bookings";
        } else {
            model.addAttribute("noApartmentAvailable", true);
            model.addAttribute("bookings", bookingRepository.findAll());
            return "bookings";
        }
    }
}