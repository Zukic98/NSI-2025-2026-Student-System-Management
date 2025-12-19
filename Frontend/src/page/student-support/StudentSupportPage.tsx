import {
  CSidebar,
  CSidebarBrand,
  CSidebarNav,
  CNavTitle,
  CNavItem,
  CNavLink,
  CContainer,
  CButton,
  CAccordion,
  CAccordionBody,
  CAccordionHeader,
  CAccordionItem,
} from "@coreui/react";

import CIcon from "@coreui/icons-react";
import {
  cilDescription,
  cilChart,
  //cilListRich,
  cilSchool,
  cilSettings,
  cilAccountLogout,
  cilInfo,
  cilChartPie,
  cilList,
} from "@coreui/icons";

import logo from '../../assets/logo-unsa-sms.png'

import "./StudentSupportPage.css";
import CategoryCard from "./CategoryCard";
import SupportRequestForm from "./SupportRequestForm";
import { useEffect, useState } from "react";

type FAQ = {
  id: number;
  question: string;
  answer: string;
};
        

export default function StudentSupportPage() {

  const [selectedCategoryId, setSelectedCategoryId] = useState<number | null>(null);
  const [faqs, setFaqs] = useState<FAQ[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetch("/api/faqs")
      .then((res) => {
        if (!res.ok) {
          throw new Error("Failed to load FAQs");
        }
        return res.json();
      })
      .then((data: FAQ[]) => {
        setFaqs(data);
        setLoading(false);
      })
      .catch((err: Error) => {
        setError(err.message);
        setLoading(false);
      });
  }, []);

  return (
    <div className="ss-page">
      <CSidebar className="ss-sidebar" unfoldable visible>
            <CSidebarBrand
                className="ss-brand"
                href="#"
                onClick={(e) => e.preventDefault()}
                >
                <img
                    src={logo}
                    alt="UNSA SMS"
                    className="ss-brandLogoFull"
                />
            </CSidebarBrand>

        <CSidebarNav className="ss-nav">
          <CNavTitle className="ss-navTitle">General</CNavTitle>

          <CNavItem>
            <CNavLink href="#" onClick={(e) => e.preventDefault()} className="ss-navLink">
              <CIcon icon={cilChartPie} className="ss-navIcon" />
              Student dashboard
            </CNavLink>
          </CNavItem>

          <CNavItem>
            <CNavLink href="#" onClick={(e) => e.preventDefault()} className="ss-navLink">
              <CIcon icon={cilDescription} className="ss-navIcon" />
              Document center
            </CNavLink>
          </CNavItem>

          <CNavItem>
            <CNavLink href="#" onClick={(e) => e.preventDefault()} className="ss-navLink">
              <CIcon icon={cilChart} className="ss-navIcon" />
              Student analytics
            </CNavLink>
          </CNavItem>

          <CNavItem>
            <CNavLink href="#" onClick={(e) => e.preventDefault()} className="ss-navLink">
              <CIcon icon={cilList} className="ss-navIcon" />
              Request management
            </CNavLink>
          </CNavItem>

          <CNavItem>
            <CNavLink href="#" onClick={(e) => e.preventDefault()} className="ss-navLink">
              <CIcon icon={cilSchool} className="ss-navIcon" />
              Enrollment
            </CNavLink>
          </CNavItem>

          <CNavTitle className="ss-navTitle">Settings</CNavTitle>

          <CNavItem>
            <CNavLink href="#" onClick={(e) => e.preventDefault()} className="ss-navLink">
              <CIcon icon={cilSettings} className="ss-navIcon" />
              Student profile settings
            </CNavLink>
          </CNavItem>

          <CNavTitle className="ss-navTitle">Help</CNavTitle>

          <CNavItem>
            <CNavLink
              href="#"
              onClick={(e) => e.preventDefault()}
              className="ss-navLink ss-active"
              active
              aria-current="page"
            >
              <CIcon icon={cilInfo} className="ss-navIcon" />
              Student support
            </CNavLink>
          </CNavItem>
        </CSidebarNav>

        <div className="ss-user">
          <div className="ss-userAvatar" aria-hidden="true">
            JS
          </div>

          <div className="ss-userMeta">
            <div className="ss-userName">Jane Smith</div>
            <div className="ss-userRole">Student</div>
          </div>

          <CButton
            type="button"
            color="link"
            className="ss-logout"
            title="Log out"
            aria-label="Log out"
          >
            <CIcon icon={cilAccountLogout} />
          </CButton>
        </div>
      </CSidebar>

      <main className="ss-main">
        <CContainer fluid className="ss-mainInner">
          <header className="ss-top">
            <h1 className="ss-title">Welcome back, Jane!</h1>
            <p className="ss-subtitle">
              Find answers or submit a request for assistance
            </p>
          </header>

          <section className="ss-content">
            <div className="ss-twoCol">
              {/* Lijevo: 4 kartice (2x2 grid) */}
              <div className="ss-categories">
                <CategoryCard
                  title="Academic support"
                  description="Questions about exams, grades or enrolment ..."
                  onClick={() => setSelectedCategoryId(1)}
                />
                <CategoryCard
                  title="Technical issues"
                  description="Platform errors, UI errors, login ..."
                  onClick={() => setSelectedCategoryId(2)}
                />
                <CategoryCard
                  title="Administrative help"
                  description="Payments, documentation ..."
                  onClick={() => setSelectedCategoryId(3)}
                />
                <CategoryCard
                  title="Account & Security"
                  description="Profile security update"
                  onClick={() => setSelectedCategoryId(4)}
                />
              </div>

              {/* Desno: prostor za formu */}
              <div className="ss-formSpace">
                <SupportRequestForm selectedCategoryId={selectedCategoryId} />
              </div>
            </div>

            {/* Ispod: prazan kontejner */}
            <div className="ss-belowEmpty">
              <section className="ss-faq">
            <h3 className="ss-faqTitle">Frequently Asked Questions</h3>

            {loading && <p>Loading FAQs...</p>}
            {error && <p>{error}</p>}

            {!loading && !error && (
              <CAccordion>
                {faqs.map((faq) => (
                  <CAccordionItem key={faq.id} itemKey={faq.id}>
                    
                    <CAccordionHeader>
                      {faq.question}
                    </CAccordionHeader>

                    <CAccordionBody>
                      {faq.answer}
                    </CAccordionBody>

                  </CAccordionItem>
                ))}
              </CAccordion>
            )}
          </section>

            </div>
          </section>

        </CContainer>
      </main>
    </div>
  );
}